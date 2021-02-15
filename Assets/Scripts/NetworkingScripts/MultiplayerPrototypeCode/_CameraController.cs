using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _CameraController : MonoBehaviour
{
    public _PlayerManager player;
    public float sensitivity = 100f;
    public float clampAngle = 85f;

    private float veritcalRotation;
    private float horizontalRotation;

    private void Start()
    {
        veritcalRotation = transform.localEulerAngles.x;
        horizontalRotation = player.transform.eulerAngles.y;
    }

    private void Update()
    {
        Look();
    }

    private void Look() 
    {
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");

        veritcalRotation += _mouseVertical * sensitivity * Time.deltaTime;
        horizontalRotation += _mouseHorizontal * sensitivity * Time.deltaTime;

        veritcalRotation = Mathf.Clamp(veritcalRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(veritcalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }
}
