using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerRestart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RestartDelay());
    }

    IEnumerator RestartDelay() 
    {
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene(0);
    }
}
