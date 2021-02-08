using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandleManager : MonoBehaviour
{
    public static void Welcome(Packet _packet) 
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from Server: {_msg}");
        ClientManager.instance.myID = _myId;
        ClientSend.WelcomeReceived();
    }
}
