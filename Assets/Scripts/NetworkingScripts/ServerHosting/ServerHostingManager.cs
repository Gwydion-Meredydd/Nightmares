using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerHostingManager : MonoBehaviour
{
    public string HostingScene;
    public string MultiplayerScene;
    public bool IsHosting;
    public ActiveScene SceneMode = ActiveScene.Client;
    public enum ActiveScene 
    {
        Client,
        Server

    };
    
    private void Awake() 
    {
        IsHosting = true;
        StartCoroutine(StartHostingScene());
    }
    IEnumerator StartHostingScene() 
    {
        while (!SceneManager.GetSceneByName(HostingScene).isLoaded) 
        {
            Debug.Log("LOADING SERVER");
            yield return null;
        }
        SwitchScene();
        yield return null;
    }
    public void SwitchScene() 
    {
        if (SceneMode == ActiveScene.Client) 
        {
            SceneMode = ActiveScene.Server;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(HostingScene));
        }
        else 
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MultiplayerScene));
            SceneMode = ActiveScene.Client;
        }
    }
}
