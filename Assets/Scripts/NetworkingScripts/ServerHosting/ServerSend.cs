using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, ServerPacket _packet)
    {
        _packet.WriteLength();
        ServerServer.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, ServerPacket _packet)
    {
        _packet.WriteLength();
        ServerServer.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(ServerPacket _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= ServerServer.MaxPlayers; i++)
        {
            ServerServer.clients[i].tcp.SendData(_packet);
        }
    }
    private static void SendTCPDataToAll(int _exceptClient, ServerPacket _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= ServerServer.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                ServerServer.clients[i].tcp.SendData(_packet);
            }
        }
    }
    private static void SendUDPDataToAll(ServerPacket _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= ServerServer.MaxPlayers; i++)
        {
            ServerServer.clients[i].udp.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, ServerPacket _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= ServerServer.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                ServerServer.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    public static void Welcome(int _toClient, string _msg)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }
    public static void SpawnPlayer(int _toClient, ServerPlayer _player)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }
    public static void PlayerPosition(ServerPlayer _Player)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.playerPosition))
        {
            _packet.Write(_Player.id);
            _packet.Write(_Player.transform.position);

            SendTCPDataToAll(_packet);
        }
    }
    public static void PlayerRotation(ServerPlayer _Player)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.playerRotation))
        {
            _packet.Write(_Player.id);
            _packet.Write(_Player.transform.rotation);

            SendTCPDataToAll(_packet);
        }
    }
    public static void SendUsernames(string _Username)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.playerJoined))
        {
            _packet.Write(_Username);
            SendTCPDataToAll(_packet);

        }

    }
    public static void LobbyIsReady(bool Ready) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.LobbyIsReady))
        {
            _packet.Write(Ready);
            SendTCPDataToAll(_packet);

        }
    }
    public static void SendReadyOrNot() 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.readyOrNot)) 
        {
            _packet.Write(ServerHostingManager.Instance.ClientReady.Count);
            for (int i = 0; i < ServerHostingManager.Instance.ClientReady.Count; i++)
            {
                _packet.Write(ServerHostingManager.Instance.ClientReady[i]);
            }
            SendTCPDataToAll(_packet);
        }
    }
    #endregion
}
