using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerModels;
using PlayFab.MultiplayerAgent.Model;

public class ServerNetworkManager : MonoBehaviour
{
    public ScriptsManager SM;

    public static ServerNetworkManager instance;

    public GameObject playerPrefab;
    public GameObject ServerLevel;
    public int Port;
    public int MaxPlayer;
    public string HostID;
    public bool LocalHosting;
    public string localIP;
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else if(instance != this) 
        {
            Debug.Log("Instance already exist, destroying object!");
            Destroy(this);
        }
    }
    public void Start()
    {
        StartServer();
    }
    public void StartServer()
    {
        if (!LocalHosting)
        {
            PlayFabMultiplayerAgentAPI.Start();
            PlayFabMultiplayerAgentAPI.IsDebugging = true;
            PlayFabMultiplayerAgentAPI.OnMaintenanceCallback += OnMaintenance;
            PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutdown;
            PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
            PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;
            StartCoroutine(ReadyForPlayers());
            StartCoroutine(ShutdownServerInXTime());
        }
        else 
        {
            LocalServerActive();
        }
    }
    IEnumerator ShutdownServerInXTime()
    {
        yield return new WaitForSeconds(300f);
        if (SM.HostingManager.ConnectedClients == 0)
        {
            if (ConsoleReader._consoleReader.IdleTurnOff)
            {
                StartShutdownProcess();
            }
        }
        else
        {
            StartCoroutine(ShutdownServerInXTime());
        }
    }
    private void OnApplicationQuit() 
    {
        ServerServer.Stop();
    }
    IEnumerator ReadyForPlayers()
    {
        yield return new WaitForSeconds(.5f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }
    public  ServerPlayer InstantiatePlayer() 
    {
        return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<ServerPlayer>();
    }
    private void OnMaintenance(DateTime? NextScheduledMaintenanceUtc)
    {
        Debug.LogFormat("Maintenance scheduled for: {0}", NextScheduledMaintenanceUtc.Value.ToLongDateString());
    }
    private void OnShutdown()
    {
        StartShutdownProcess();
    }

    private void StartShutdownProcess()
    {
        Debug.Log("Server is shutting down");
        StartCoroutine(ShutdownServer());
    }
    private void LocalServerActive() 
    {
        Port = 7777;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        ServerServer.Start(MaxPlayer, Port, localIP);
        Debug.Log("ServerStart");
        Debug.Log("Server Started From Agent Activation");
    }
    private void OnServerActive()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        ServerServer.Start(MaxPlayer, Port, HostID);
        Debug.Log("ServerStart");
        Debug.Log("Server Started From Agent Activation");
    }
    private void OnAgentError(string error)
    {
        Debug.Log(error);
    }
    IEnumerator ShutdownServer()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}
