using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public ScriptsManager SM;

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
        }
        else 
        {
            SM.GameScript.Paused = true;
        }

    }
}
