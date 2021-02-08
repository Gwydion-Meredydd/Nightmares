using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NightMaresServer
{
    class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;


        private static TcpListener tcpListener;

        public static void Start(int _MaxPlayers, int _port) 
        {
            MaxPlayers = _MaxPlayers;
            Port = _port;

            Console.WriteLine("Starting Server...");
            InitializeServerData();
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TPCConnectCallBack),null);

            Console.WriteLine($"Server Started on {Port}.");
        }
        private static void TPCConnectCallBack(IAsyncResult _result) 
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TPCConnectCallBack),null);
            Console.WriteLine($"Incomming Connection from {_client.Client.RemoteEndPoint}...");
            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null) 
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }
            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server Full!");
        }
        private static void InitializeServerData() 
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomeReceived,ServerHandle.WelcomeReceived }
            };
            Console.WriteLine("Initialized packets.");
        }
    }
}
