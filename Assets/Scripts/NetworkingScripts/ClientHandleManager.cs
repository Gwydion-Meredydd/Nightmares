using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientHandleManager : MonoBehaviour
{
    public static void Welcome(Packet _packet) 
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from Server: {_msg}");
        ClientManager.instance.myID = _myId;
        ClientSend.WelcomeReceived();

        ClientManager.instance.udp.Connect(((IPEndPoint)ClientManager.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
    public static void SpawnPlayer(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        _GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }
    public static void PlayerPosition(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        _GameManager.players[_id].transform.position = _position;
    }
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        _GameManager.players[_id].transform.rotation = _rotation;
    }
}
