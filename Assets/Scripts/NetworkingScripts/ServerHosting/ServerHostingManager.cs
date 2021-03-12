using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ServerHostingManager : MonoBehaviour
{
    [SerializeField]
    public ServerHostingManager InstanceHolder;
    public static ServerHostingManager Instance;

    public string HostingScene;
    public string MultiplayerScene;
    public bool IsHosting;
    public ActiveScene SceneMode = ActiveScene.Client;

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
    public enum ActiveScene
    {
        Client,
        Server

    };
    private void Awake()
    {
        Instance = InstanceHolder;
        IsHosting = true;
        StartCoroutine(StartHostingScene());
    }

    private void FixedUpdate()
    {
        if (!SpawnGame)
        {
            if (ConnectedClients > 1)
            {
                EnoughPlayerHaveJoined = true;
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
        Instantiate(ServerNetworkManager.instance.ServerLevel, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
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
            yield return null;
        }
        if (AllPlayersReady && EnoughPlayerHaveJoined && CountDownInitilised && CountDownTime > StartCountDownTime)
        {
            Debug.Log("SPAWN LEVEL & PLAYERS");
            SpawnGame = true;
        }
    }
    IEnumerator StartHostingScene() 
    {
        while (!SceneManager.GetSceneByName(HostingScene).isLoaded) 
        {
            Debug.Log("LOADING SERVER");
            yield return null;
        }
        SwitchScene();
        yield return null;
    }
    public void SwitchScene() 
    {
        if (SceneMode == ActiveScene.Client) 
        {
            SceneMode = ActiveScene.Server;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(HostingScene));
        }
        else 
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MultiplayerScene));
            SceneMode = ActiveScene.Client;
        }
    }
    public void SwitchSceneToServer()
    {
        SceneMode = ActiveScene.Server;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(HostingScene));
    }
    public void SwitchSceneToClient()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(MultiplayerScene));
        SceneMode = ActiveScene.Client;
    }
    public void RemoveClient(int OldClientValue)
    {
        ConnectedClientsClass.RemoveAt(OldClientValue);
        ConnectedClientsIP.RemoveAt(OldClientValue);
        ConnectedClientsUsernames.RemoveAt(OldClientValue);
        ConnectedClients = ConnectedClients - 1;
    }
}
