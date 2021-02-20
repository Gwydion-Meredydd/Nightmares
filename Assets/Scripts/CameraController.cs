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
    RaycastHit RayCastHit;
    public SkinnedMeshRenderer[] EnemySkinedMeshRenderes;


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
                if (Physics.Raycast(SM.PlayerScript.HeightOcclusionPoint.transform.position, SM.PlayerScript.HeightOcclusionPoint.transform.forward, out RayCastHit ,100))
                {
                    if (RayCastHit.transform.CompareTag("Player") || RayCastHit.transform.CompareTag("Enemy"))
                    {
                        foreach (var HeightBlocker in SM.LevelScript.HeightOcclusionObjects)
                        { 
                            HeightBlocker.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        }
                        foreach (var HeightBlocker in SM.LevelScript.HeightOcclusionReplacementObjects)
                        {
                            HeightBlocker.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                        }
                        foreach (GameObject AboveEnemy in SM.LevelScript.HeightOccludedEnemies)
                        {
                            Debug.Log("Normal skin");
                            EnemySkinedMeshRenderes = AboveEnemy.GetComponentsInChildren<SkinnedMeshRenderer>();
                            foreach (SkinnedMeshRenderer SkinRenders in EnemySkinedMeshRenderes)
                            {
                                SkinRenders.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                            }
                            EnemySkinedMeshRenderes = null;
                        }
                    }
                    else
                    {
                        foreach (var HeightBlocker in SM.LevelScript.HeightOcclusionObjects)
                        {
                            HeightBlocker.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                        }
                        foreach (var HeightBlocker in SM.LevelScript.HeightOcclusionReplacementObjects)
                        {
                            HeightBlocker.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        }
                        foreach (GameObject AboveEnemy in SM.LevelScript.HeightOccludedEnemies)
                        {
                            Debug.Log("Transpent skin");
                            EnemySkinedMeshRenderes = AboveEnemy.GetComponentsInChildren<SkinnedMeshRenderer>();
                            foreach (SkinnedMeshRenderer SkinRenders in EnemySkinedMeshRenderes)
                            {
                                SkinRenders.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                            }
                            EnemySkinedMeshRenderes = null;
                        }
                    }
                }
            }
        }
        else if (SM.GameScript.Server) 
        {
            if (SM.Client_Manager.HasJoined)
            {
                if (SM.GameScript.CameraInstantiated)
                {
                    if (Camera.transform.position != player.position)
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
}
