using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public ScriptsManager SM;
    public bool InActiveRound;
    public bool StartingNewRound;
    [Range(0, 10)]
    public float StartDelay;
    public int RoundNumber;
    
    IEnumerator NewRound() 
    {
        StartingNewRound = true;
        RoundNumber = RoundNumber + 1;
        SM.GameMenuScript.RoundText.text = RoundNumber.ToString();
        SM.AudioScripts.GameSFXAudioSource.PlayOneShot(SM.AudioScripts.NewRound);
        yield return new WaitForSecondsRealtime(StartDelay);
        StartingNewRound = false;
        InActiveRound = true;
    }
}
