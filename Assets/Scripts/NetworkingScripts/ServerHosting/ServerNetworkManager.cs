using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerNetworkManager : MonoBehaviour
{
    public static ServerNetworkManager instance;

    public GameObject playerPrefab;
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
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        ServerServer.Start(4, 27005);

    }
    private void OnApplicationQuit() 
    {
        ServerServer.Stop();
    }

    public ServerPlayer InstantiatePlayer() 
    {
        return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<ServerPlayer>();
    }
}
