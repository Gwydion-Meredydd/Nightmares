﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

public class ServerServerHandle
{
    public static void WelcomeReceived(int _fromClient, ServerPacket _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        Debug.Log(_fromClient);
   
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player\"{_username}\"(ID: {_fromClient}) has assumed the wrong client ID({_clientIdCheck})!");
        }
        else
        {
            Debug.Log($"{ServerServer.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            ServerHostingManager.Instance.ConnectedClientsUsernames.Add(_username);
            ServerHostingManager.Instance.ConnectedClients = ServerHostingManager.Instance.ConnectedClients + 1;
            ServerHostingManager.Instance.ConnectedClientsIP.Add(ServerServer.clients[_fromClient].tcp.socket.Client.RemoteEndPoint.ToString());
            ServerHostingManager.Instance.ClientReady.Add(false);
            ServerServer.clients[_fromClient].SendUsernames();
        }
    }
    public static void ClientNeedsPlayer(int _fromClient, ServerPacket _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        ServerServer.clients[_fromClient].SendIntoGame(_username);
    }
    public static void PlayerMovement(int _fromClient, ServerPacket _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        ServerServer.clients[_fromClient].player.SetInputs(_inputs);
    }
    public static void PlayerRotation(int _fromClient, ServerPacket _packet)
    {
        Vector3 NewRotation = _packet.ReadVector3();
        ServerServer.clients[_fromClient].player.SetRotation(NewRotation);
    }
    public static void PlayerIsReady(int _fromClient , ServerPacket _packet) 
    {
        ServerHostingManager.Instance.ClientReady[_fromClient - 1] = true;
        ServerServer.clients[_fromClient].ReadyToggle();
    }
    public static void PlayerNotReady(int _fromClient, ServerPacket _packet)
    {
        ServerHostingManager.Instance.ClientReady[_fromClient - 1] = false;
        ServerServer.clients[_fromClient].ReadyToggle();
    }

}