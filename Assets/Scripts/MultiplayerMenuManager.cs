using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenuManager : MonoBehaviour
{
    public static MultiplayerMenuManager instance;

    public GameObject StartMenu;
    public string UserName;

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
    public void ConnectedToServer()
    {
        StartMenu.SetActive(false);
        ClientManager.instance.ConnectToServer();
    }
}
