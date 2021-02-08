using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet) 
    {
        _packet.WriteLength();
        ClientManager.instance.tcp.SendData(_packet);
    }
    #region Packet
    
    public static void WelcomeReceived() 
    {
        using(Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(ClientManager.instance.myID);
            _packet.Write(MultiplayerMenuManager.instance.UserName);

            SendTCPData(_packet);
        }
    }

    #endregion
}
