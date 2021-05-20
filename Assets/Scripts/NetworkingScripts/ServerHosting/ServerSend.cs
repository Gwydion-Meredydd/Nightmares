using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, ServerPacket _packet)
    {
        _packet.WriteLength();
        //ServerServer.
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
    public static void NewWeaponValue(int ID, ServerPlayer _Player) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.WeaponValueToClient))
        {
            _packet.Write(_Player.id);
            _packet.Write(_Player.WeaponValue);
            SendTCPDataToAll(_packet);
        }
    }
    public static void PlayerShoot(ServerPlayer _Player) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.MouseSendToPlayer))
        {
            _packet.Write(_Player.id);
            _packet.Write(_Player.Firing);

            SendTCPDataToAll(_packet);
        }
    }
    public static void SpawnGroundEnemy(Quaternion EnemyRotation, Vector3 EnemyPosition, int randomValue) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.EnemyGroundSpawn))
        {
            _packet.Write(EnemyRotation);
            _packet.Write(EnemyPosition);
            _packet.Write(randomValue);

            SendTCPDataToAll(_packet);
        }
    }
    public static void SpawnNormalEnemy(Quaternion EnemyRotation, Vector3 EnemyPosition, int randomValue)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.EnemyNormalSpawn))
        {
            _packet.Write(EnemyRotation);
            _packet.Write(EnemyPosition);
            _packet.Write(randomValue);

            SendTCPDataToAll(_packet);
        }
    }
    public static void SendEnemyTransform(Quaternion[] NewEnemyRotation, Vector3[] NewEnemyPosition) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.EnemyTransformSend))
        {
            _packet.Write(NewEnemyRotation.Length);
            foreach (Quaternion Rotation in NewEnemyRotation)
            {
                _packet.Write(Rotation);
            }
            foreach (Vector3 Position in NewEnemyPosition)
            {
                _packet.Write(Position);
            }

            SendTCPDataToAll(_packet);
        }
    }
    public static void SendEnemyDamage(int ArrayValue , int NewDamage) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.EnemyDamaged))
        {
            _packet.Write(ArrayValue);
            _packet.Write(NewDamage);

            SendTCPDataToAll(_packet);
        }
    }
    public static void SendEnemyAttack(int ArrayValue)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.EnemyAttack))
        {
            _packet.Write(ArrayValue);
            SendTCPDataToAll(_packet);
        }
    }
    public static void SendEnemyHitPlayer(int PlayerID, float NewHealth)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.EnemyHit))
        {
            _packet.Write(PlayerID);
            _packet.Write(NewHealth);
            SendTCPDataToAll(_packet);
        }
    }
    public static void SendPlayerRevive(int PlayerID)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.PlayerRevive))
        {
            _packet.Write(PlayerID);
            SendTCPDataToAll(_packet);
        }
    }
    public static void DropDropped(int DropValue,Vector3 Position, bool DropStatus)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.DropDropped))
        {
            _packet.Write(DropValue);
            _packet.Write(Position);
            _packet.Write(DropStatus);
            SendTCPDataToAll(_packet);
        }
    }
    public static void DropData(int DropValue,bool DropStatus) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.DropPicked))
        {
            _packet.Write(DropValue);
            _packet.Write(DropStatus);
            SendTCPDataToAll(_packet);
        }
    }
    public static void PlayerHasDisconnected(int ClientValue) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.PlayerDisconnected))
        {
            _packet.Write(ClientValue);
            SendTCPDataToAll(_packet);
        }
    }
    public static void NewRound(int RoundValue) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.NewRound))
        {
            _packet.Write(RoundValue);
            SendTCPDataToAll(_packet);
        }
    }
    public static void SendScore(int PlayerID, int Score)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.NewScore))
        {
            _packet.Write(PlayerID);
            _packet.Write(Score);
            SendTCPDataToAll(_packet);
        }
    }
    public static void SendPerkValue (int PerkValue)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.NewPerkValue))
        {
            _packet.Write(PerkValue);
            SendTCPDataToAll(_packet);
        }
    }
    public static void SendPerkData(int ID, bool PerkStatus, string PerkMessage)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.PerkData))
        {
            _packet.Write(PerkStatus);
            _packet.Write(PerkMessage);
            SendTCPData(ID, _packet);
        }
    }
    public static void SendBaughtPerk(int ID,int PerkValue)
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.PerkBaught))
        {
            _packet.Write(ID);
            _packet.Write(PerkValue);
            SendTCPDataToAll(_packet);
        }
    }
    public static void UpdateScore(int ID,int NewScore) 
    {
        using (ServerPacket _packet = new ServerPacket((int)ServerServerPackets.ScoreUpdate))
        {
            _packet.Write(ID);
            _packet.Write(NewScore);
            SendTCPDataToAll(_packet);
        }
    }
    #endregion
}
