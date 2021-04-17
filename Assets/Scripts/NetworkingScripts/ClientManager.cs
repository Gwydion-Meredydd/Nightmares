using System.Collections;
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
   // [HideInInspector]
    public string ip;
    //[HideInInspector]
    public int port ;
    public int myID = 0;
    public TCP tcp;
    public UDP udp;

    public bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int,PacketHandler> packetHandlers;
    public bool LocalConnection;
    public bool CreatingLobby;
    public bool ShowServerDetail;
    public Text ServerDetail;
    public bool[] ServersActive;
    private bool VirutalServerBoolsInstantiated;
    private int ServerSelectionValue;
    public int VirtualServerSelectionValue;

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
        if (ShowServerDetail == true) { ServerDetail.gameObject.SetActive(true); }
        else { ServerDetail.gameObject.SetActive(false); }

    }
    #region GET CONFIG 
    private void Update()
    {
        if (SM.ConfigManager.GetConfigPortandIp) 
        {
            SM.ConfigManager.GetConfigPortandIp = false;
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true,
                CustomId = GUIDUtility.getUniqueID()
            };

            PlayFabClientAPI.LoginWithCustomID(request, GettingConfig, GettingConfigError);
        }
    }
    private void GettingConfig(LoginResult response) 
    {
        Debug.Log(response.ToString());
        FetchServerRequest();
    }
    private void PrintConfig(RequestMultiplayerServerResponse response = null) 
    {
        Debug.Log("**** Adding host ip and host port to client -- IP: " + response.IPV4Address + " Port: " + (ushort)response.Ports[0].Num);
    }
    private void GettingConfigError(PlayFabError response) 
    {
        Debug.Log(response.ToString());
    }
    private void FetchServerRequest() 
    {
        RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest();
        requestData.BuildId = buildId;
        requestData.SessionId = System.Guid.NewGuid().ToString();
        requestData.PreferredRegions = new List<AzureRegion>() { AzureRegion.NorthEurope };
        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, SucccsessServerRequest, OnRequestServerError);
    }
    private void SucccsessServerRequest(RequestMultiplayerServerResponse response)
    {
        Debug.Log(response.ToString());
        PrintConfig(response);
    }
    private void OnRequestServerError(PlayFabError error)
    {
        Debug.Log(error.ToString());
    }

    #endregion

    #region Login TO Playfab
    public void LoginToPlayfab()
    {
        if (!LocalConnection)
        {
            SM.multiplayerMenuManager.WaitingOnServerInfoText("Logging into Server");
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
        else 
        {
            ConnectLocalClient();
        }
    }
    private void OnLoginError(PlayFabError response)
    {
        SM.multiplayerMenuManager.WaitingForServer.SetActive(false);
        SM.multiplayerMenuManager.ServerFullErrorToggle();
        Debug.Log(response.ToString());
    }
    private void OnPlayFabLoginSuccess(LoginResult response)
    {
        SM.multiplayerMenuManager.WaitingOnServerInfoText("Loggin Succsess");
        Debug.Log(response.ToString() + "Succsess");
        ip = ip = SM.ConfigManager.ip;
        port = SM.ConfigManager.port;
        CheckServersAreActive();
    }
    #endregion

    #region Request All Server Data
    private void CheckServersAreActive() 
    {
        RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest();
        requestData.BuildId = buildId;
        requestData.SessionId = System.Guid.NewGuid().ToString();
        requestData.PreferredRegions = new List<AzureRegion>() { AzureRegion.NorthEurope };
        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, ServerWasRequested, ServerWasActive);
    }
    private void ServerWasRequested(RequestMultiplayerServerResponse response)
    {
        StartCoroutine(ServerWasRequestedTime(response));
    }
    IEnumerator ServerWasRequestedTime(RequestMultiplayerServerResponse response) 
    {
        SM.multiplayerMenuManager.WaitingOnServerInfoText("Fetching Servers...");
        Debug.Log("Server Found, Port: " + (ushort)response.Ports[0].Num);
        yield return new WaitForSecondsRealtime(4);
        CheckServersAreActive();
    }
    private void ServerWasActive(PlayFabError error) 
    {
        SM.multiplayerMenuManager.WaitingOnServerInfoText("Server Data Ready...");
        StartLoginProcess();
    }
    #endregion

    #region Find Available Server
    private void StartLoginProcess() 
    {
        SM.multiplayerMenuManager.WaitingOnServerInfoText("Starting Connection...");
        ServerSelectionValue = 0;
        VirtualServerSelectionValue = 0;
        port = SM.ConfigManager.port;
        Debug.Log("Attempting Connection");
        InitalizeClientData();
        AttemptLogin();
    }
    private void AttemptLogin() 
    {
        tcp = new TCP();
        isConnected = true;
        tcp.Connect();
        StartCoroutine(LoginTimeOut());
    }
    IEnumerator LoginTimeOut() 
    {
        if (!LocalConnection)
        {
            yield return new WaitForSecondsRealtime(7);
            if (!HasJoined)
            {
                Debug.Log(ServerSelectionValue + " " + VirtualServerSelectionValue);
                if (VirtualServerSelectionValue < SM.ConfigManager.VirtualServerAmmount - 1)
                {
                    VirtualServerSelectionValue = VirtualServerSelectionValue + 1;
                    port = port + VirtualServerSelectionValue;
                    AttemptLogin();
                }
                else
                {
                    ServerSelectionValue = ServerSelectionValue + 1;
                    if (ServerSelectionValue < SM.ConfigManager.ServerAmmount)
                    {
                        port = port + 100;
                        VirtualServerSelectionValue = 0;
                        AttemptLogin();
                    }
                    else
                    {
                        if (!HasJoined)
                        {
                            ServerSelectionValue = 0;
                            VirtualServerSelectionValue = 0;
                            SM.multiplayerMenuManager.ServerFullErrorToggle();
                            SM.multiplayerMenuManager.WaitingForServer.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                ServerSelectionValue = 0;
                VirtualServerSelectionValue = 0;
                SM.multiplayerMenuManager.WaitingForServer.SetActive(false);
                if (ShowServerDetail)
                {
                    ServerDetail.text = "Port:" + port.ToString();
                }
            }
        }
        else 
        {
            yield return new WaitForSecondsRealtime(3);
            if (!HasJoined) 
            {
                SM.multiplayerMenuManager.ServerFullErrorToggle();
            }
            else 
            {
                SM.multiplayerMenuManager.WaitingForServer.SetActive(false);
            }
        }
    }
    #endregion

    #region Local Connection
    private void ConnectLocalClient()
    {
        ip = "127.0.0.1";
        port = 7777;
        Debug.Log("Connection started..");
        SM.multiplayerMenuManager.WaitingForServer.SetActive(true);
        InitalizeClientData();
        tcp = new TCP();
        isConnected = true;
        tcp.Connect();
        StartCoroutine(LoginTimeOut());
    }
    #endregion

    #region ClientServerCommunication
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
            {(int) ServerPackets.WeaponValueToClient, ClientHandleManager.RecievedWeaponValue },
            {(int) ServerPackets.MouseSendToPlayer, ClientHandleManager.ServerShootingRecevied },
            {(int) ServerPackets.EnemyGroundSpawn, ClientHandleManager.SpawnGroundEnemy },
            {(int) ServerPackets.EnemyNormalSpawn, ClientHandleManager.SpawnNormalEnemy },
            {(int) ServerPackets.EnemyTransformSend, ClientHandleManager.ReceviedEnemyTransform },
        };
        Debug.Log("Initalized Packets!");
    }
    private void OnApplicationQuit()
    {
        DisconnectClient();
    }
    public void DisconnectClient()
    {
        Disconnect();
    }
    public void NewWeaponValueRecevied(int NewId, int NewWeaponValue) 
    {
        SM.GameScript.NewWeaponValue(NewId, NewWeaponValue);
    }
    public void ShootingServerRecevied(int NewId, bool IsFiring) 
    {
        SM.GameScript.ShootingServerRecevied(NewId, IsFiring);
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
    private void Disconnect() 
    {
        if (isConnected) 
        {
            HasJoined = false;
            isConnected = false;
            tcp.socket.Close();
            myID = 0;
        }
        SM.multiplayerManager.Disconnect();
        SM.multiplayerMenuManager.Disconnct();
        Debug.Log("Disconnected from server.");
    }
    #endregion
}

