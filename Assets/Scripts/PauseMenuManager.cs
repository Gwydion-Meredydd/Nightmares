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
    public bool ScreenMode = false;
    private void Start()
    {
        FullScreenToggle();
    }
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
    public void FullScreenToggle() 
    {
        if (!ScreenMode) 
        {
            ScreenMode = true;
            Screen.fullScreen = Screen.fullScreen;
        }
        else 
        {
            ScreenMode = false;
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
    public void ResolutionDropDown(int DropDownValue) 
    {
        switch (DropDownValue) 
        {
            case 0:
                Screen.SetResolution(1080, 720, ScreenMode);
                break;
            case 1:
                Screen.SetResolution(1280, 1080, ScreenMode);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, ScreenMode);
                break;
        }
    }
    public void QualityDropDown(int DropDownValue) 
    {
        switch (DropDownValue)
        {
            case 0:
                QualitySettings.SetQualityLevel(0, true);
                break;
            case 1:
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 2:
                QualitySettings.SetQualityLevel(5, true);
                break;
        }
    }
    public void MasterSlider(float SliderValue)
    {
        Mixer.SetFloat("Master", Mathf.Log10(SliderValue) * 20);
    }
    public void AudioSlider(float SliderValue)
    {
        Mixer.SetFloat("Audio", Mathf.Log10(SliderValue) * 20);
    }
    public void MusicSlider(float SliderValue)
    {
        Mixer.SetFloat("Music", Mathf.Log10(SliderValue) * 20);
    }

    public void MainMenuReturn() 
    {
        SM.PlayerScript.PlayerDead();
    }
}
