using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    public ScriptsManager SM;
    public GameObject PauseMenu;
    public GameObject PausedSubMenu;
    public GameObject OptionsSubMenu;
    public GameObject OptionsAudio;
    public GameObject OptionsVideo;
    [Space]
    public AudioMixer Mixer;
    public void Update()
    {
        if (SM.GameScript.InGame) 
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                PauseToggle();
            }
        }
    }
    public void PauseToggle() 
    {
        if (SM.GameScript.Paused) 
        {
            SM.GameScript.Paused = false;
            SM.GameMenuScript.InGameMenu.SetActive(true);
            PauseMenu.SetActive(false);
        }
        else 
        {
            SM.GameScript.Paused = true;
            SM.GameMenuScript.InGameMenu.SetActive(false);
            PauseMenu.SetActive(true);
            PausedSubMenu.SetActive(true);
            OptionsSubMenu.SetActive(false);
            OptionsAudio.SetActive(false);
            OptionsVideo.SetActive(false);
        }
    }
    public void OptionsMenuToggle() 
    {
        if (OptionsSubMenu.activeInHierarchy) 
        {
            OptionsSubMenu.SetActive(false);
            PausedSubMenu.SetActive(true);
        }
        else
        {
            OptionsSubMenu.SetActive(true);
            PausedSubMenu.SetActive(false);
        }
    }
    public void AudioOptions()
    {
        if (!OptionsAudio.activeInHierarchy)
        {
            OptionsAudio.SetActive(true);
            OptionsVideo.SetActive(false);
        }
        else 
        {
            OptionsAudio.SetActive(false);
            OptionsVideo.SetActive(false);
        }
    }
    public void VideoOptions()
    {
        if (!OptionsVideo.activeInHierarchy)
        {
            OptionsAudio.SetActive(false);
            OptionsVideo.SetActive(true);
        }
        else 
        {
            OptionsAudio.SetActive(false);
            OptionsVideo.SetActive(false);
        }
    }
    public void MainMenuReturn() 
    {
        SM.PlayerScript.PlayerDead();
    }
}
