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

    private void Awake()
    {
        Instance = InstanceHolder;
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
        try 
        {
            Instantiate(ServerNetworkManager.instance.ServerLevel, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
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
            yield return new  WaitForSeconds(0.01f);
        }
        if (AllPlayersReady && EnoughPlayerHaveJoined && CountDownInitilised && CountDownTime > StartCountDownTime)
        {
            Debug.Log("SPAWN LEVEL & PLAYERS");
            SpawnGame = true;
        }
    }
    public void RemoveClient(int OldClientValue)
    {
        ConnectedClientsClass.RemoveAt(OldClientValue);
        ConnectedClientsIP.RemoveAt(OldClientValue);
        ConnectedClientsUsernames.RemoveAt(OldClientValue);
        ConnectedClients = ConnectedClients - 1;
    }
}
