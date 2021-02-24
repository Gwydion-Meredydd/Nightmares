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

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        ClientManager.instance.udp.SendData(_packet);
    }

    #region Packet

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(ClientManager.instance.myID);
            _packet.Write(MultiplayerMenuManager.instance.UserName);

            SendTCPData(_packet);
        }
    }
    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            SendUDPData(_packet);
        }
    }
    public static void PlayerRotation(Vector3 PlayerRotation) 
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerRotation))
        {
            _packet.Write(PlayerRotation);
            //_packet.Write(GameManager.players[ClientManager.instance.myID].transform.rotation);

            SendUDPData(_packet);
        }
    }
    #endregion
}
