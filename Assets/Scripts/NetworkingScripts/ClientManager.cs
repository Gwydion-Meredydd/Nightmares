using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class ClientManager : MonoBehaviour
{
    public static ClientManager instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 27005;
    public int myID = 0;
    public TCP tcp;

    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int,PacketHandler> packetHandlers;

    private void Awake()
    {
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
        tcp = new TCP();
    }

    public void ConnectToServer()
    {
        InitalizeClientData();
        tcp.Connect();
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
                    return;
                }
                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch 
            {
                
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
    }
    private void InitalizeClientData() 
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int) ServerPackets.welcome,ClientHandleManager.Welcome}
        };
        Debug.Log("Initalized Packets!");
    }
}

