using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    public MultiplayerManager _instnace;
    public static MultiplayerManager instance;
    [Space]
    public string[] Username;
    public bool[] IsReady;

    private void Start()
    {
        instance = _instnace;
    }
    public void Disconnect() 
    {
        Username = new string[4];
        IsReady = new bool[4];
        foreach (var Username in SM.multiplayerMenuManager.Usernames)
        {
            Username.text = "";
        }
        SM.multiplayerMenuManager.UpdateReadUpToggle(4);

    }
}
