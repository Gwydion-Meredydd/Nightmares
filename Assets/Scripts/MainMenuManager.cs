﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public ScriptsManager SM;
    public GameObject mainMenuObj;
    public GameObject optionsMenuObj;
    public GameObject OptionsAudio;
    public GameObject OptionsVideo;
    public GameObject CharacterGameobjects;
    public GameObject CharactersOption;
    public GameObject LevelGameObjects;
    public GameObject LevelSelectionOption;
    public GameObject HighScoreObj;
    public GameObject HighScoreLoading;
    [Space]
    public InputField UsernameInputField;
    public string UserName;
    public bool UserNameEntered;
    public GameObject[] UserNameErrorText;
    public GameObject Loading;
    public void PlayButton()
    {
        mainMenuObj.SetActive(true);
        if (UserNameEntered)
        {
            CharacterSelection();
        }
        else 
        {
            StartCoroutine(NoUsernameEntered(0));
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
    public void HighScoreToggle() 
    {
        if (HighScoreObj.activeInHierarchy) 
        {
            SM.ScoreScript.YourScore.SetActive(false);
            SM.ScoreScript.HighScoreScore.text = "";
            SM.ScoreScript.HighScoreScore.text = "";
            HighScoreObj.SetActive(false);
        }
        else 
        {
            HighScoreObj.SetActive(true);
            SM.ScoreScript.RefreshLeaderboardOnUpload();
        }
    }
    public void CharacterSelectionReturn() 
    {
        mainMenuObj.SetActive(true);
        CharactersOption.SetActive(false);
        CharacterGameobjects.SetActive(false);
    }
    public void CharacterSelection() 
    {
        mainMenuObj.SetActive(false);
        CharactersOption.SetActive(true);
        CharacterGameobjects.SetActive(true);
    }
    public void CharacterSelected(int SelectionOption) 
    {
        SM.GameScript.PlayerTypeValue = SelectionOption;
        LevelSelectionOption.SetActive(true);
        LevelGameObjects.SetActive(true);
        CharactersOption.SetActive(false);
        CharacterGameobjects.SetActive(false);
    }
    public void MapSelectedReturn()
    {
        LevelSelectionOption.SetActive(false);
        LevelGameObjects.SetActive(false);
        CharactersOption.SetActive(true);
        CharacterGameobjects.SetActive(true);
    }
    public void MapSelected(int LevelValue) 
    {
        LevelSelectionOption.SetActive(false);
        LevelGameObjects.SetActive(false);
        SM.LevelScript.LevelValue = LevelValue;
        SM.GameScript.StartGame = true;
    }
    public void UserNameFinished(InputField NewInputField) 
    {
        Loading.SetActive(true);
        UserNameEntered = false;
        UserName = NewInputField.text.ToString();
        Debug.Log(UserName);
        SM.ScoreScript.CheckUserNameFromMainMenu(UserName);
    }
    public void ServerIsDown()
    {
        UserNameEntered = true;
        Loading.SetActive(false);
        StartCoroutine(NoUsernameEntered(4));
    }
    public void UserNameIsOkay() 
    {
        UserNameEntered = true;
        Loading.SetActive(false);
        StartCoroutine(NoUsernameEntered(3));
    }
    public void UserNameNotAvailable()
    {
        StartCoroutine(NoUsernameEntered(1));
        UsernameInputField.text = "Enter UserName";
        Loading.SetActive(false);
    }
    public void UserNameNotSuitable() 
    {
        StartCoroutine(NoUsernameEntered(2));
        UsernameInputField.text = "Enter UserName";
        Loading.SetActive(false);
    }
    public void OptionsButton()
    {
        Debug.Log("Options Menu");
        mainMenuObj.SetActive(false);
        optionsMenuObj.SetActive(true);
    }
    public void BackButton()
    {
        Debug.Log("Back to Main Menu");
        mainMenuObj.SetActive(true);
        optionsMenuObj.SetActive(false);
        OptionsAudio.SetActive(false);
        OptionsVideo.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");

        Application.Quit();
    }
    IEnumerator NoUsernameEntered(int TextValue)
    {
        UserNameErrorText[TextValue].SetActive(true);
        yield return new WaitForSeconds(3);
        UserNameErrorText[TextValue].SetActive(false);
    }
}
