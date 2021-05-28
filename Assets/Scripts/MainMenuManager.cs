using System.Collections;
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
    public GameObject HighScoreRoot;
    public GameObject[] HighScorePerent;
    public GameObject[] HighScoreLoading;
    public GameObject HighScoreButtons;
    public GameObject ThreeDimensionalCharacters;
    public GameObject ThreeDimensionalBackground;
    [Space]
    public InputField UsernameInputField;
    public string UserName;
    public bool UserNameEntered;
    public GameObject[] UserNameErrorText;
    public GameObject Loading;
    public GameObject MainMenuScene;
    public GameObject ScoreOverlayBlocker;
    public GameObject ScoreOverlayBlack;
    public GameObject ScoreReturnButton;
    public GameObject ScoreMenuButtons;
    public void PlayButton()
    {
        mainMenuObj.SetActive(true);
        if (UserNameEntered)
        {
            CharacterSelection();
        }
        else 
        {
            NoUserNameEnteredMethod();
        }

    }
    public void NoUserNameEnteredMethod() 
    {
        StartCoroutine(NoUsernameEntered(0));
    }
    public void AudioOptions()
    {
        SM.AudioScripts.PlayMenuSFX();
        if (!OptionsAudio.activeInHierarchy)
        {
            OptionsAudio.SetActive(true);
            OptionsVideo.SetActive(false);
        }
        else
        {
            OptionsAudio.SetActive(true);
            OptionsVideo.SetActive(false);
        }
    }
    public void VideoOptions()
    {
        SM.AudioScripts.PlayMenuSFX();
        if (!OptionsVideo.activeInHierarchy)
        {
            OptionsAudio.SetActive(false);
            OptionsVideo.SetActive(true);
        }
        else
        {
            OptionsAudio.SetActive(false);
            OptionsVideo.SetActive(true);
        }
    }
    public void HighScoreToggle() 
    {
        SM.AudioScripts.PlayMenuSFX();
        if (HighScoreRoot.activeInHierarchy) 
        {
            SM.ScoreScript.YourScore1.SetActive(false);
            SM.ScoreScript.HighScoreScore1.text = "";
            SM.ScoreScript.HighScoreScore1.text = "";
            HighScoreRoot.SetActive(false);
            HighScorePerent[0].SetActive(false);
            HighScorePerent[1].SetActive(false);
            //HighScorePerent[2].SetActive(false);
            //HighScorePerent[3].SetActive(false);
            HighScorePerent[4].SetActive(false);
            HighScoreButtons.SetActive(false);
        }
        else 
        {
            HighScoreRoot.SetActive(true);
            HighScoreButtons.SetActive(true);
            SM.ScoreScript.RefreshLeaderboardOnUpload();
        }
    }
    public void HighSocreValueToggle(int HighScoreValue) 
    {
        SM.AudioScripts.PlayMenuSFX();
        SM.ScoreScript.LeaderboardValue = HighScoreValue;
        HighScorePerent[HighScoreValue - 1].SetActive(true);
        SM.ScoreScript.RefreshLeaderboardOnUpload();
    }
    public void CharacterSelectionReturn() 
    {
        ThreeDimensionalCharacters.SetActive(true);
        SM.AudioScripts.PlayMenuSFX();
        mainMenuObj.SetActive(true);
        CharactersOption.SetActive(false);
        CharacterGameobjects.SetActive(false);
    }
    public void CharacterSelection() 
    {
        ThreeDimensionalCharacters.SetActive(false);
        SM.AudioScripts.PlayMenuSFX();
        mainMenuObj.SetActive(false);
        CharactersOption.SetActive(true);
        CharacterGameobjects.SetActive(true);
    }
    public void CharacterSelected(int SelectionOption) 
    {
        SM.AudioScripts.PlayMenuSFX();
        SM.GameScript.PlayerTypeValue = SelectionOption;
        LevelSelectionOption.SetActive(true);
        LevelGameObjects.SetActive(true);
        CharactersOption.SetActive(false);
        CharacterGameobjects.SetActive(false);
    }
    public void MapSelectedReturn()
    {
        SM.AudioScripts.PlayMenuSFX();
        LevelSelectionOption.SetActive(false);
        LevelGameObjects.SetActive(false);
        CharactersOption.SetActive(true);
        CharacterGameobjects.SetActive(true);
    }
    public void MapSelected(int LevelValue) 
    {
        MainMenuScene.SetActive(false);
        SM.AudioScripts.PlayMenuSFX();
        LevelSelectionOption.SetActive(false);
        LevelGameObjects.SetActive(false);
        ScoreOverlayBlocker.SetActive(false);
        ScoreReturnButton.SetActive(false);
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
        SM.AudioScripts.PlayMenuSFX();
        Debug.Log("Options Menu");
        OptionsAudio.SetActive(true);
        OptionsVideo.SetActive(false);
        mainMenuObj.SetActive(false);
        optionsMenuObj.SetActive(true);
    }
    public void BackButton()
    {
        SM.AudioScripts.PlayMenuSFX();
        Debug.Log("Back to Main Menu");
        mainMenuObj.SetActive(true);
        optionsMenuObj.SetActive(false);
        OptionsAudio.SetActive(false);
        OptionsVideo.SetActive(false);
    }
    public void FeedBackForm() 
    {
        Application.OpenURL("https://docs.google.com/forms/d/1k_HUUW28pz8jhA1aJ00VMRoMgnfKfL1q1dr490Lxozo/edit");
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
