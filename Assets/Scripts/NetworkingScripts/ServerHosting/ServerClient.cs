using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

[System.Serializable]
public class ServerClient
{
    public static ServerClient instance;
    public static int dataBufferSize = 4096;

    public int id;
    public ServerPlayer player;
    public TCP tcp;
    public UDP udp;


    public ServerClient(int _clientID)
    {
        id = _clientID;
        tcp = new TCP(id);
        udp = new UDP(id);
    }
    public class TCP
    {
        public TcpClient socket;
        private NetworkStream stream;
        private ServerPacket receivedData;
        private byte[] receiveBuffer;

        private readonly int id;

        public TCP(int _id)
        {
            id = _id;
        }
        public void Connect(TcpClient _Socket)
        {
            socket = _Socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receivedData = new ServerPacket();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);

            ServerSend.Welcome(id, "Welcome to the Server!");

        }
        public void SendData(ServerPacket _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallBack(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    ServerServer.clients[id].Disconnect();
                    return;
                }
                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP data: {_ex}");
                ServerServer.clients[id].Disconnect();
            }
        }
        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }
            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ServerThreadManager.ExecuteOnMainThread(() =>
                {
                    using (ServerPacket _packet = new ServerPacket(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        ServerServer.packetHandlers[_packetId](id, _packet);
                    }
                });
                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }
            if (_packetLength <= 1)
            {
                return true;
            }
            return false;
        }
        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UDP(int _id)
        {
            id = _id;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
        }

        public void SendData(ServerPacket _packet)
        {
            ServerServer.SendUDPData(endPoint, _packet);
        }
        public void HandleData(ServerPacket _packetData)
        {
            int _packetLength = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

            ServerThreadManager.ExecuteOnMainThread(() =>
            {
                using (ServerPacket _packet = new ServerPacket(_packetBytes))
                {
                    int _packetId = _packet.ReadInt();
                    ServerServer.packetHandlers[_packetId](id, _packet);
                }
            });
        }
        public void Disconnect()
        {
            endPoint = null;
        }
    }

    public void SendIntoGame(string _playerName)
    {
        Debug.Log("send into game");
        player = ServerNetworkManager.instance.InstantiatePlayer();
        player.Initialize(id, _playerName);
        foreach (ServerClient _client in ServerServer.clients.Values)
        {
            if (_client.player != null)
            {
                if (_client.id != id)
                {
                    ServerSend.SpawnPlayer(id, _client.player);
                }
            }
        }
        foreach (ServerClient _client in ServerServer.clients.Values)
        {
            if (_client.player != null)
            {
                ServerSend.SpawnPlayer(_client.id, player);
            }
        }
    }
    public void SendUsernames() 
    {
        string Usernames;
        switch (ServerHostingManager.Instance.ConnectedClients) 
        {
            case 0:
                Usernames = "";
                ServerSend.SendUsernames(Usernames);
                break;
            case 1:
                Usernames = ServerHostingManager.Instance.ConnectedClientsUsernames[0];
                    ServerSend.SendUsernames(Usernames);
                break;
            case 2:
                Usernames = ServerHostingManager.Instance.ConnectedClientsUsernames[0] +"*"
                    + ServerHostingManager.Instance.ConnectedClientsUsernames[1];
                    ServerSend.SendUsernames(Usernames);
                break;
            case 3:
                Usernames = ServerHostingManager.Instance.ConnectedClientsUsernames[0] + "*"
                    + ServerHostingManager.Instance.ConnectedClientsUsernames[1] + "*"
                    + ServerHostingManager.Instance.ConnectedClientsUsernames[2];
                    ServerSend.SendUsernames(Usernames);
                break;
            case 4:
                Usernames = ServerHostingManager.Instance.ConnectedClientsUsernames[0] + "*"
                    + ServerHostingManager.Instance.ConnectedClientsUsernames[1] + "*"
                    + ServerHostingManager.Instance.ConnectedClientsUsernames[2] + "*"
                    + ServerHostingManager.Instance.ConnectedClientsUsernames[3];
                    ServerSend.SendUsernames(Usernames);
                break;
        }
    }
    public void ReadyToggle() 
    {
        ServerSend.SendReadyOrNot();
    }
    private void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has disconnected.");

        ServerThreadManager.ExecuteOnMainThread(() =>
            {

                if (player != null)
                {
                    UnityEngine.Object.Destroy(player.gameObject);
                    player = null;
                }
            });
        int ClientValue = 0;
        foreach (string ClientIP in ServerHostingManager.Instance.ConnectedClientsIP)
        {  
            if (ClientIP == tcp.socket.Client.RemoteEndPoint.ToString()) 
            {
                Debug.Log("Client Number " + ClientValue + " Has Disconnect from the server...");
                ServerHostingManager.Instance.ConnectedClientsIP.RemoveAt(ClientValue);
                ServerHostingManager.Instance.ConnectedClientsUsernames.RemoveAt(ClientValue);
                ServerHostingManager.Instance.ConnectedClients = ServerHostingManager.Instance.ConnectedClients - 1;
                break;
            }
            ClientValue = ClientValue + 1;
        }
        SendUsernames();
        tcp.Disconnect();
        udp.Disconnect();
    }
}
