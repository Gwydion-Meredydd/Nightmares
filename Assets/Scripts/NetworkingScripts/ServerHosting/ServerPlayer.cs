using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayer: MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public float moveSpeed = 0.1f;
    private bool[] inputs;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        ServerHostingManager.Instance.ConnectedClientsClass.Add(this);
        ServerHostingManager.Instance.ConnectedClients = ServerHostingManager.Instance.ConnectedClients + 1;
        inputs = new bool[4];
    }
    public void FixedUpdate()
    {
        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y = -1;
        }
        if (inputs[2])
        {
            _inputDirection.x = -1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }
        Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = new Vector3 (_inputDirection.x, -1 , _inputDirection.y);
        controller.Move((_moveDirection * moveSpeed)*Time.fixedUnscaledDeltaTime);

        ServerSend.PlayerPosition(this);

    }

    public void SetInputs(bool[] _inputs)
    {
        inputs = _inputs;
    }
    public void SetRotation(Vector3 Rotation) 
    {
        this.transform.LookAt(Rotation);
        ServerSend.PlayerRotation(this);
    }
}
