using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameManager game_Manager;
    public PlayerController player_Controller;
    public Transform player;
    public float smoothing;
    public Transform Camera;
    public float yValue;
    public Vector3 maxPosition;
    public Vector3 minPosition;


    void Start()
    {

    }

    void LateUpdate()
    {
        if (game_Manager.InGame == true)
        {
            if (game_Manager.Paused == false)
            {
                if (transform.position != player.position)
                {
                    Vector3 targetPosition = new Vector3(player.position.x,
                                                           yValue,
                                                           player.transform.position.z);

                    targetPosition.x = Mathf.Clamp(targetPosition.x,
                                                    minPosition.x,
                                                    maxPosition.x);
                    targetPosition.z = Mathf.Clamp(targetPosition.z,
                                                  minPosition.z,
                                                  maxPosition.z);

                    Camera.transform.position = Vector3.Lerp(Camera.position,
                                                      targetPosition,
                                                      smoothing);
                }
            }
        }


    }
}
