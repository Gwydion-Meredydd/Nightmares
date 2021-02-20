using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerHostingManager : MonoBehaviour
{
    public string HostingScene;
    public void HostScene() 
    {
        SceneManager.LoadScene(HostingScene, LoadSceneMode.Additive);
    }
}
