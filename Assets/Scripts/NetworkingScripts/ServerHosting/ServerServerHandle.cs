﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ServerServerHandle
{
    public static void WelcomeReceived(int _fromClient, ServerPacket _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{ServerServer.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player\"{_username}\"(ID: {_fromClient}) has assumed the wrong client ID({_clientIdCheck})!");
        }
        ServerServer.clients[_fromClient].SendIntoGame(_username);
    }
    public static void PlayerMovement(int _fromClient, ServerPacket _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        ServerServer.clients[_fromClient].player.SetInputs(_inputs, _rotation);
    }
}