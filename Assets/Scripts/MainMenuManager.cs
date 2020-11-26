using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuObj;
    public GameObject optionsMenuObj;

    public void StartGame()
    {
        Debug.Log("Starting Game");
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
}
