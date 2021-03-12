using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerMenuManager : MonoBehaviour
{
    public static MultiplayerMenuManager instance;

    public GameObject StartMenu;
    public string UserName;

    public Text[] Usernames;
    public Toggle[] ReadyUpBox;

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
    public void HostScene() 
    {
        SceneManager.LoadScene("HostingScene", LoadSceneMode.Additive);
    }
    public void UpdateReadUpToggle(int ConnectedCount) 
    {
        Debug.Log("CALLING METHOD" + ConnectedCount);
        switch (ConnectedCount) 
        {
            case 1:
                if (MultiplayerManager.instance.IsReady[0] == true) 
                {
                    ReadyUpBox[0].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[0] == false)
                {
                    ReadyUpBox[0].isOn = false;
                }
                break;
            case 2:
                if (MultiplayerManager.instance.IsReady[0] == true)
                {
                    ReadyUpBox[0].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[0] == false)
                {
                    ReadyUpBox[0].isOn = false;
                }
                if (MultiplayerManager.instance.IsReady[1] == true)
                {
                    ReadyUpBox[1].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[1] == false)
                {
                    ReadyUpBox[1].isOn = false;
                }
                break;
            case 3:
                if (MultiplayerManager.instance.IsReady[0] == true)
                {
                    ReadyUpBox[0].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[0] == false)
                {
                    ReadyUpBox[0].isOn = false;
                }
                if (MultiplayerManager.instance.IsReady[1] == true)
                {
                    ReadyUpBox[1].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[1] == false)
                {
                    ReadyUpBox[1].isOn = false;
                }
                if (MultiplayerManager.instance.IsReady[2] == true)
                {
                    ReadyUpBox[2].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[2] == false)
                {
                    ReadyUpBox[2].isOn = false;
                }
                break;
            case 4:
                if (MultiplayerManager.instance.IsReady[0] == true)
                {
                    ReadyUpBox[0].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[0] == false)
                {
                    ReadyUpBox[0].isOn = false;
                }
                if (MultiplayerManager.instance.IsReady[1] == true)
                {
                    ReadyUpBox[1].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[1] == false)
                {
                    ReadyUpBox[1].isOn = false;
                }
                if (MultiplayerManager.instance.IsReady[2] == true)
                {
                    ReadyUpBox[2].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[2] == false)
                {
                    ReadyUpBox[2].isOn = false;
                }
                if (MultiplayerManager.instance.IsReady[3] == true)
                {
                    ReadyUpBox[3].isOn = true;
                }
                else if (MultiplayerManager.instance.IsReady[3] == false)
                {
                    ReadyUpBox[3].isOn = false;
                }
                break;
        }
    }
}
