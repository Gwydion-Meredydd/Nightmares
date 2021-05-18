using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientConfigManager : MonoBehaviour
{
    [HideInInspector]
    public static ClientConfigManager _clientConfigManager;
    [HideInInspector]
    public ClientConfigManager RefClientConfigManger;

    public string buildId;
    public string ip;
    public int port;
    public int ServerAmmount;
    public int VirtualServerAmmount;
    public bool GetConfigPortandIp;

    [Header("Port Forward Hositng")]
    public string IP;
    public int PORT;


    private void Start()
    {
        RefClientConfigManger = this;
        _clientConfigManager = RefClientConfigManger;
    }
}