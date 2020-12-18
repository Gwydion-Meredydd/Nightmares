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
    [Space]
    public InputField UsernameInputField;
    public string UserName;
    public bool UserNameEntered;
    public GameObject[] UserNameErrorText;
    public GameObject Loading;
    public void StartGame()
    {
        mainMenuObj.SetActive(true);
        if (UserNameEntered)
        {
            SM.GameScript.StartGame = true;
        }
        else 
        {
            StartCoroutine(NoUsernameEntered(0));
        }

    }
    public void UserNameFinished(InputField NewInputField) 
    {
        Loading.SetActive(true);
        UserNameEntered = false;
        UserName = NewInputField.text.ToString();
        Debug.Log(UserName);
        SM.ScoreScript.CheckUserNameFromMainMenu(UserName);
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

    public void Leaderboards()
    {
        Debug.Log("Leaderboards Loading");
    }

    public void BackButton()
    {
        Debug.Log("Back to Main Menu");
        mainMenuObj.SetActive(true);
        optionsMenuObj.SetActive(false);
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
