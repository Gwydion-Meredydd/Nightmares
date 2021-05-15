using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ServerHostingManager : MonoBehaviour
{
    public ScriptsManager SM;
    [SerializeField]
    public ServerHostingManager InstanceHolder;
    public static ServerHostingManager Instance;

    public int ConnectedClients;
    public List<string> ConnectedClientsUsernames;
    public List<string> ConnectedClientsIP;
    public List<ServerPlayer> ConnectedClientsClass;
    public List<bool> ClientReady;
    public bool EnoughPlayerHaveJoined;
    public bool AllPlayersReady;
    public bool CountDownInitilised;
    public float StartCountDownTime;
    public float CountDownTime;
    public bool SpawnGame;
    public bool SpawnLevel;
    public bool AFKInstantiated;
    public int AFKStartingTime;
    public int AFKTime;
    public bool DataCheckTimerIsWorking;
    public string[] OldConnectedClientsUsernames;
    public string[] OldConnectedClientsIP;
    public bool[] oldClientReady;
    private bool AutoShutDownStarted;
    public GameObject SpawnedLevel;
    public Vector3 MaxPosition;
    public Vector3 MinPosition;

    private void Awake()
    {
        Instance = InstanceHolder;
        AFKTime = AFKStartingTime;
    }

    private void FixedUpdate()
    {
        if (!AutoShutDownStarted) 
        {
            AutoShutDownStarted = true;
            StartCoroutine(AutoShutDown());
        }
        if (!SpawnGame)
        {
            if (AFKTime < 1)
            {
                Debug.Log("Disconnecting Clients");
                DisconnectAllClient();
            }
            if (ConnectedClients > 1)
            {
                EnoughPlayerHaveJoined = true;
                if (AFKInstantiated == false) 
                {
                    RefreshOldData();
                    AFKInstantiated = true;
                }
            }
            else
            {
                EnoughPlayerHaveJoined = false;
            }
            if (!AllPlayersReady)
            {
                if (EnoughPlayerHaveJoined)
                {
                    for (int i = 0; i < ClientReady.Count; i++)
                    {
                        if (ClientReady[i] == false)
                        {
                            AllPlayersReady = false;
                            CountDownInitilised = false;
                            return;
                        }
                    }
                    AllPlayersReady = true;
                }
            }
            else
            {
                if (!EnoughPlayerHaveJoined)
                {
                    AllPlayersReady = false;
                    CountDownInitilised = false;
                }
                else
                {
                    for (int i = 0; i < ClientReady.Count; i++)
                    {
                        if (ClientReady[i] == false)
                        {
                            AllPlayersReady = false;
                            CountDownInitilised = false;
                            return;
                        }
                    }
                }
                if (!CountDownInitilised)
                {
                    CountDown();
                }
            }
        }
        else
        {
            if (!SpawnLevel)
            {
                InstantiateLevel();
            }
        }
    }
    public void InstantiateLevel()
    {
        try
        {
            SpawnedLevel =  Instantiate(ServerNetworkManager.instance.ServerLevel, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            GameObject[] TempSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            SM.serverEnemySpawner.LevelSpawnPoints = new Transform[TempSpawnPoints.Length];
            for (int i = 0; i < TempSpawnPoints.Length; i++)
            {
                SM.serverEnemySpawner.LevelSpawnPoints[i] = TempSpawnPoints[i].transform;
            }
            SM.serverNavMeshBuilder.LevelReadyForNavmesh();
        }
        catch
        {
            Debug.Log("failed to spawn collisons...");
        }
        SpawnLevel = true;
        ServerSend.LobbyIsReady(true);
    }
    public void CountDown()
    {
        CountDownTime = 0;
        CountDownInitilised = true;
        StartCoroutine(TimeCountDown());
    }
    IEnumerator TimeCountDown()
    {
        while (AllPlayersReady && EnoughPlayerHaveJoined && CountDownInitilised && CountDownTime < StartCountDownTime)
        {
            CountDownTime = CountDownTime + 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        if (AllPlayersReady && EnoughPlayerHaveJoined && CountDownInitilised && CountDownTime > StartCountDownTime)
        {
            Debug.Log("SPAWN LEVEL & PLAYERS");
            SpawnGame = true;
        }
    }
    public void OldDataCheck() 
    {
        if (OldConnectedClientsUsernames.Length == ConnectedClientsUsernames.Count)
        {
            for (int i = 0; i < ConnectedClientsUsernames.Count; i++)
            {
                if (OldConnectedClientsUsernames[i] == ConnectedClientsUsernames[i])
                {
                    if (OldConnectedClientsIP[i] == ConnectedClientsIP[i])
                    {
                        if (oldClientReady[i] == ClientReady[i])
                        {
                            if (!DataCheckTimerIsWorking)
                            {
                                DataCheckTimerIsWorking = true;
                                StartCoroutine(DataCheckTimer());
                            }
                        }
                        else
                        {
                            RefreshOldData();
                        }
                    }
                    else
                    {
                        RefreshOldData();
                    }
                }
                else
                {
                    RefreshOldData();
                }
            }
        }
        else 
        {
            RefreshOldData();
        }
    }
    IEnumerator DataCheckTimer() 
    {
        yield return new WaitForSecondsRealtime(1);
        AFKTime = AFKTime - 1;
        //Debug.Log("afk time left: " + AFKTime);
        DataCheckTimerIsWorking = false;
        if (ConnectedClients != 0) 
        {
            OldDataCheck();
        }
    }
    public void RefreshOldData() 
    {
        AFKTime = AFKStartingTime;
        OldConnectedClientsUsernames = new string[ConnectedClientsUsernames.Count];
        OldConnectedClientsIP = new string[ConnectedClientsUsernames.Count];
        oldClientReady = new bool[ConnectedClientsUsernames.Count];
        for (int i = 0; i < ConnectedClientsUsernames.Count; i++)
        {
            OldConnectedClientsUsernames[i] = ConnectedClientsUsernames[i];
            OldConnectedClientsIP[i] = ConnectedClientsIP[i];
            oldClientReady[i] = ClientReady[i];
        }
        OldDataCheck();
    }
    IEnumerator AutoShutDown()
    {
        yield return new WaitForSeconds(500f);
        if (ConnectedClients != 0) 
        {
            StartCoroutine(AutoShutDown());
        }
        else 
        {
            Application.Quit();
        }
    }
    public void DisconnectAllClient() 
    {
        ConnectedClientsIP.Clear();
        ConnectedClientsUsernames.Clear();
        ClientReady.Clear();
        Debug.Log("AFK Server kicked all clients");
        Application.Quit();
    }
}
