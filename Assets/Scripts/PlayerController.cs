using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager Game_Manager;

    public GameObject Player;
    void Update()
    {
        if (Game_Manager.InGame == true) 
        {
            if (Game_Manager.Paused == false) 
            {
                PlayerInputs();
            }
        }
    }
    public void PlayerInputs() 
    {
        MousePlayerRotation();
    }
    public void MousePlayerRotation()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            Player.transform.LookAt(new Vector3(hit.point.x, Player.transform.position.y, hit.point.z));
        }
    }
}
