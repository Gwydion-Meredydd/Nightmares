using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerRaycast : MonoBehaviour
{
    public RaycastHit NewRayCastHit;

    public GameObject HitGameObject;
    public bool Shoot;
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out NewRayCastHit))
        {
            HitGameObject = NewRayCastHit.transform.gameObject;
        }
    }
}
