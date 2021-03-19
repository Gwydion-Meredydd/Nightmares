﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;

public class ClientManager : MonoBehaviour
{
    public ScriptsManager SM;
    public  bool HasJoined;
    public static ClientManager instance;
    public static int dataBufferSize = 4096;

    [HideInInspector]
    public string buildId = "";
    [HideInInspector]
    public string ip;
    [HideInInspector]
    public int port ;
    public int myID = 0;
    public TCP tcp;
    public UDP udp;

    public bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int,PacketHandler> packetHandlers;

    private void Awake()
    {
        buildId = SM.ConfigManager.buildId;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destryoing Object!");
            Destroy(this);
        }
    }
    private void Start()
    {
        //tcp = new TCP();
        //udp = new UDP();
    }

    private void OnApplicationQuit()
    {
        DisconnectClient();
    }
    public void DisconnectClient() 
    {
        Disconnect();
    }
    public void ClientReadyToggle(Text ButtonText)  
    {
        if (HasJoined)
        {
            SM.multiplayerMenuManager.WaitingForServer.SetActive(true);
            if (ButtonText.text == "READY!")
            {
                ClientUnReady();
                ButtonText.text = "Not-Ready";
            }
            else if (ButtonText.text == "Not-Ready")
            {
                ButtonText.text = "READY!";
                ClientReady();
            }
        }
    }
    public void ClientReady() 
    {
        ClientSend.PlayerIsReady();
    }
    public void ClientUnReady() 
    {
        ClientSend.PlayerNotReady();
    }

    public void HostConnectToServer()
    {
        SM.multiplayerMenuManager.WaitingForServer.SetActive(true);
        Debug.Log("[ClientStartUp].LoginRemoteUser");

        //We need to login a user to get at PlayFab API's. 
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = GUIDUtility.getUniqueID()
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabHostLoginSuccess, OnLoginError);
    }
    public void ConnectToServer()
    {
        SM.multiplayerMenuManager.WaitingForServer.SetActive(true);
        Debug.Log("[ClientStartUp].LoginRemoteUser");

        //We need to login a user to get at PlayFab API's. 
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = GUIDUtility.getUniqueID()
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabLoginSuccess, OnLoginError);
    }
    private void OnLoginError(PlayFabError response)
    {
        Debug.Log(response.ToString());
    }
    private void OnPlayFabLoginSuccess(LoginResult response)
    {
        Debug.Log(response.ToString());
        ip = ip = SM.ConfigManager.ip;
        port = SM.ConfigManager.port;
        ConnectRemoteClient();
    }
    private void OnPlayFabHostLoginSuccess(LoginResult response)
    {
        Debug.Log(response.ToString());

        RequestMultiplayerServer();
    }
    private void RequestMultiplayerServer()
    {
        Debug.Log("[ClientStartUp].RequestMultiplayerServer");
        RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest();
        requestData.BuildId = buildId;
        requestData.SessionId = System.Guid.NewGuid().ToString();
        requestData.PreferredRegions = new List<AzureRegion>() { AzureRegion.NorthEurope };
        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
    }
    private void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
    {
        Debug.Log(response.ToString());
        ConnectRemoteClient(response);
    }
    private void ConnectRemoteClient(RequestMultiplayerServerResponse response = null)
    {
        if (response == null)
        {
            ip = ip = SM.ConfigManager.ip;
            port = SM.ConfigManager.port;
            Debug.Log("joining made lobby");
        }
        else
        {
            Debug.Log("**** Adding host ip and host port to client -- IP: " + response.IPV4Address + " Port: " + (ushort)response.Ports[0].Num);
            ip = response.IPV4Address;
            port = (ushort)response.Ports[0].Num;
        }
        Debug.Log("Connection started..");
        InitalizeClientData();
        tcp = new TCP();
        udp = new UDP();
        isConnected = true;
        tcp.Connect();
    }
    private void OnRequestMultiplayerServerError(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
        SM.multiplayerMenuManager.ServerFullErrorToggle();
    }
    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;
        public void Connect()
        {

            socket = new TcpClient
            {
               
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };
            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallBack, socket);
        }
        private void ConnectCallBack(IAsyncResult _result) 
        {
            socket.EndConnect(_result);
            if (!socket.Connected) 
            {
                return;
            }
            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer,0,dataBufferSize,ReceiveCallback,null);
        }
        public void SendData(Packet _packet) 
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
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }
        private void ReceiveCallback(IAsyncResult _result) 
        {
            try 
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }
                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch 
            {
                Disconnect();
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
                ThreadManager.ExecuteOnMainThread(() => 
                {
                    using (Packet _packet = new Packet(_packetBytes)) 
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
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
        private void Disconnect() 
        {
            instance.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP 
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP() 
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }
        public void Connect(int _localPort) 
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet()) 
            {
                SendData(_packet);
            }
        }
        public void SendData(Packet _packet) 
        {
            try 
            {
                _packet.InsertInt(instance.myID); 
                if (socket != null) 
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result) 
        {
            try 
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4) 
                {
                    instance.Disconnect();
                    return;
                }
                HandleData(_data);
            }
            catch 
            {
                Disconnect();
            }
        }

        private void HandleData(byte[] _data) 
        {
            using (Packet _packet = new Packet(_data)) 
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data)) 
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });
        }
        
        private void Disconnect() 
        {
            instance.Disconnect();

            endPoint = null;
            socket = null;
        }
    }

    private void InitalizeClientData() 
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int) ServerPackets.welcome,ClientHandleManager.Welcome},
            {(int) ServerPackets.playerJoined, ClientHandleManager.PlayerJoined },
            {(int) ServerPackets.readyOrNot, ClientHandleManager.ReadyOrNot },
            {(int) ServerPackets.LobbyIsReady, ClientHandleManager.LobbyisReady },
            {(int) ServerPackets.spawnPlayer, ClientHandleManager.SpawnPlayer },
            {(int) ServerPackets.playerPosition, ClientHandleManager.PlayerPosition },
            {(int) ServerPackets.playerRotation, ClientHandleManager.PlayerRotation },
        };
        Debug.Log("Initalized Packets!");
    }

    private void Disconnect() 
    {
        if (isConnected) 
        {
            HasJoined = false;
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnected from server.");
        }
    }
}

