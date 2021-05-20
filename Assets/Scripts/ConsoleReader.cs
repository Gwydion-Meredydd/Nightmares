using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleReader : MonoBehaviour
{
    public ScriptsManager SM;
    public static ConsoleReader _consoleReader;
    public  ConsoleReader RefconsoleReader;

    public GameObject Canvas;
    public bool IdleTurnOff;

    public Toggle InGame;
    public int ConnectedPlayersValue;
    public Text[] Usernames;
    public string[] UsernameStrings;
    private void Start()
    {
        RefconsoleReader = this;
        _consoleReader = RefconsoleReader;
        StartCoroutine(UpdateClock());
    }

    IEnumerator UpdateClock() 
    {
        yield return new WaitForSecondsRealtime(5f);
       // Canvas.SetActive(false);
        yield return new WaitForEndOfFrame();
        if (ServerHostingManager.Instance.ConnectedClientsUsernames.Count > 0)
        {
            UsernameStrings[0] = ServerHostingManager.Instance.ConnectedClientsUsernames[0].ToString();
            ConnectedPlayersValue = 1;
            if (ServerHostingManager.Instance.ConnectedClientsUsernames.Count > 1)
            {
                UsernameStrings[1] = ServerHostingManager.Instance.ConnectedClientsUsernames[1].ToString();
                ConnectedPlayersValue = 2;
                if (ServerHostingManager.Instance.ConnectedClientsUsernames.Count > 2)
                {
                    UsernameStrings[2] = ServerHostingManager.Instance.ConnectedClientsUsernames[2].ToString();
                    ConnectedPlayersValue = 3;
                    if (ServerHostingManager.Instance.ConnectedClientsUsernames.Count > 3)
                    {
                        UsernameStrings[3] = ServerHostingManager.Instance.ConnectedClientsUsernames[3].ToString();
                        ConnectedPlayersValue = 4;
                    }
                }
            }
            InGame.isOn = SM.HostingManager.SpawnLevel;
        }
        else
        {
            Usernames[0].text = "Empty Connection Slot";
            Usernames[1].text = "Empty Connection Slot";
            Usernames[2].text = "Empty Connection Slot";
            Usernames[3].text = "Empty Connection Slot";
        }

        Usernames[0].text = UsernameStrings[0].ToString();
        Usernames[1].text = UsernameStrings[1].ToString();
        Usernames[2].text = UsernameStrings[2].ToString();
        Usernames[3].text = UsernameStrings[3].ToString();
        yield return new WaitForEndOfFrame();
        //Canvas.SetActive(true);
        yield return new WaitForEndOfFrame();
        StartCoroutine(UpdateClock());
    }
    public void ToggleAutoTurnOff(bool NewiNPUT) 
    {
        IdleTurnOff = NewiNPUT;
    }
}
