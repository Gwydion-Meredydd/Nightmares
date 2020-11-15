using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    public Transform player;
    public float smoothing;
    public Transform Camera;
    public float yValue;
    public float HoldingYValue;
    public Vector3 maxPosition;
    public Vector3 minPosition;

    void LateUpdate()
    {
        if (SM.GameScript.InGame == true)
        {
            if (SM.GameScript.Paused == false)
            {
                if (transform.position != player.position)
                //is the camera in the same position as the player
                {
                    Vector3 targetPosition = new Vector3(player.position.x,
                                                           yValue,
                                                           player.transform.position.z);
                    //keep the x and z value the same as the player while holding the same position on the y

                    targetPosition.x = Mathf.Clamp(targetPosition.x,
                                                    minPosition.x,
                                                    maxPosition.x);
                    //lock the camera x axis so it can't go outside the bounds of the level
                    targetPosition.z = Mathf.Clamp(targetPosition.z,
                                                  minPosition.z,
                                                  maxPosition.z);
                    //lock the camera z axis so it can't go outside the bounds of the level

                    Camera.transform.position = Vector3.Lerp(Camera.position,
                                                      targetPosition,
                                                      smoothing);
                    //creates slight lag in the camera follow movement to make it seem smoother
                }
            }
        }


    }
}
