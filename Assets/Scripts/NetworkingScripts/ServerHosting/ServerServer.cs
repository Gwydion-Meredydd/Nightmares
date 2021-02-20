using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ServerServer
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, ServerClient> clients = new Dictionary<int, ServerClient>();
    public delegate void PacketHandler(int _fromClient, ServerPacket _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;


    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int _MaxPlayers, int _port)
    {
        MaxPlayers = _MaxPlayers;
        Port = _port;

        Debug.Log("Starting Server...");
        InitializeServerData();
        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TPCConnectCallBack), null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);


        Debug.Log($"Server Started on {Port}.");

    }
    private static void TPCConnectCallBack(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TPCConnectCallBack), null);
        Debug.Log($"Incomming Connection from {_client.Client.RemoteEndPoint}...");
        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(_client);
                return;
            }
        }
        Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect: Server Full!");
    }

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }

            using (ServerPacket _packet = new ServerPacket(_data))
            {
                int _clientId = _packet.ReadInt();

                if (_clientId == 0)
                {
                    return;
                }

                if (clients[_clientId].udp.endPoint == null)
                {
                    clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error receiving UDP data:{_ex}");
        }
    }

    public static void SendUDPData(IPEndPoint _clientEndPoint, ServerPacket _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
        }
    }

    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new ServerClient(i));
        }
        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerClientPackets.welcomeReceived,ServerServerHandle.WelcomeReceived },
                {(int)ServerClientPackets.playerMovement,ServerServerHandle.PlayerMovement },
            };
        Debug.Log("Initialized packets.");
    }

    public static void Stop() 
    {
        tcpListener.Stop();
        udpListener.Close();
    }
}
