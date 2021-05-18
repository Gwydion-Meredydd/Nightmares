using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerMenuManager : MonoBehaviour
{
    public ScriptsManager SM;
    public static MultiplayerMenuManager instance;

    public GameObject WaitingForServer;
    public Text WaitingForServerInfoText;
    public GameObject ServerFullError;

    public GameObject MultiplayerCanvas;
    public GameObject MultiplayerStartingUI;
    public GameObject MultiplayerGameObjectPassword;
    public GameObject HostLocalMultiplayerMenu;
    public string HostLocalDownloadurl;
    public GameObject StartMenu;
    public GameObject MainMultiMenu;
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
    public void SetMultiplayerUIActive()
    {
        if (SM.MainMenuScript.UserNameEntered)
        {
            SM.multiplayerMenuManager.UserName = SM.MainMenuScript.UserName.ToString();
            SM.MainMenuScript.mainMenuObj.SetActive(false);
            MultiplayerGameObjectPassword.SetActive(false);
            MultiplayerCanvas.SetActive(true);
            MainMultiMenu.SetActive(false);
            StartMenu.SetActive(true);
            WaitingForServer.SetActive(false);
            ServerFullError.SetActive(false);
        }
        else 
        {
            SM.MainMenuScript.NoUserNameEnteredMethod();
        }
    }
    public void LocalJoinButtonPressed() 
    {
        ClientManager.instance.LocalConnection = true;
        GameManager.instance.Server = true;
        ConnectedToServer();
    }
    public void HostLocalMenu() 
    {
        MultiplayerStartingUI.SetActive(false);
        HostLocalMultiplayerMenu.SetActive(true);
    }
    public void HostLocalDownload() 
    {
        Application.OpenURL(HostLocalDownloadurl);
        HostLocalMenuButtonReturn();
    }
    public void HostLocalMenuButtonReturn()
    {
        MultiplayerStartingUI.SetActive(true);
        HostLocalMultiplayerMenu.SetActive(false);
    }
    public void MultiplayerButtonPressed() 
    {
        MultiplayerStartingUI.SetActive(false);
        MultiplayerGameObjectPassword.SetActive(true);
    }
    public void MultiplayerButtonReturn()
    {
        MultiplayerStartingUI.SetActive(true);
        MultiplayerGameObjectPassword.SetActive(false);
    }
    public void PasswordEntered(string PasswordInput) 
    {
        if (PasswordInput.ToLower() == "unity>unreal") 
        {
            MultiplayerGameObjectPassword.SetActive(false);
            Debug.Log("PasswordEntered");
            ClientManager.instance.LocalConnection = false;
            GameManager.instance.Server = true;
            ConnectedToServer();
        }
    }
    public void ServerFullErrorToggle()
    {
        if (ServerFullError.activeInHierarchy)
        {
            ServerFullError.SetActive(false);
        }
        else
        {
            WaitingForServer.SetActive(false);
            ServerFullError.SetActive(true);
        }
        StartCoroutine(ServerFullErrorTimeout());
    }
    public void WaitingOnServerToggle() 
    {
        if (WaitingForServer.activeInHierarchy) 
        {
            WaitingForServer.SetActive(false);
        }
        else 
        {
            WaitingForServer.SetActive(true);
        }
    }
    public void WaitingOnServerInfoText(string Info) 
    {
        WaitingForServerInfoText.text = Info;
    }
    public void ConnectedToServer()
    {
        StartMenu.SetActive(false);
        MainMultiMenu.SetActive(true);
        ClientManager.instance.LoginToPlayfab();
    }
    public void HostScene() 
    {
        SceneManager.LoadScene("HostingScene", LoadSceneMode.Additive);
    }
    public void UpdateReadUpToggle(int ConnectedCount) 
    {
        WaitingForServer.SetActive(false);
        Debug.Log("CALLING METHOD" + ConnectedCount);
        foreach (Toggle ReadyUpbox in ReadyUpBox)
        {
            ReadyUpbox.isOn = false;
        }
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
    IEnumerator ServerFullErrorTimeout()
    {
        yield return new WaitForSecondsRealtime(3);
        ServerFullError.SetActive(false);
        WaitingForServer.SetActive(false);
        if (!SM.Client_Manager.HasJoined)
        {
            StartMenu.SetActive(true);
            if (MultiplayerGameObjectPassword != null)
            {
                MultiplayerGameObjectPassword.SetActive(false);
            }
            MainMultiMenu.SetActive(false);
        }
    }
    public void Disconnct()
    {
        ServerFullError.SetActive(false);
        WaitingForServer.SetActive(false);
        StartMenu.SetActive(true);
        MainMultiMenu.SetActive(false);
    }
}
