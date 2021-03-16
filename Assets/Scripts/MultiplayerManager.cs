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
}
