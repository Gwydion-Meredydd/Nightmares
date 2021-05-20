using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientHandleManager : MonoBehaviour
{
    public ScriptsManager SM;
    public bool HasJoined;
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        ClientManager.instance.myID = _myId;
        ClientManager.instance.HasJoined = true;
        ClientManager.instance.SM.multiplayerMenuManager.WaitingForServer.SetActive(false);
        ClientManager.instance.SM.multiplayerMenuManager.WaitingForServerInfoText.text = "";
        ClientSend.WelcomeReceived();

        //ClientManager.instance.udp.Connect(((IPEndPoint)ClientManager.instance.tcp.socket.Client.LocalEndPoint).Port);
        if (!ClientManager.instance.LocalConnection)
        {
            ClientManager.instance.SM.multiplayerMenuManager.WaitingOnServerToggle();
        }
    }
    public static void SpawnPlayer(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
        GameManager.instance.SM.LevelScript.SpawnMultiplayerLevel();
    }
    public static void PlayerPosition(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        
        GameManager.players[_id].transform.position = Vector3.Lerp(GameManager.players[_id].transform.position, _position, Time.fixedDeltaTime * 10);
    }
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }
    public static void PlayerJoined(Packet _packet) 
    {
        string PacketData = _packet.ReadString().ToString();
        string[] SplitData = PacketData.Split(new char[] { '*' }, System.StringSplitOptions.RemoveEmptyEntries);
        MultiplayerManager.instance.Username = new string[SplitData.Length];
        foreach (var username in MultiplayerMenuManager.instance.Usernames)
        {
            username.text = "";
        } 
        switch (SplitData.Length) 
        {

            case 1:
                MultiplayerManager.instance.Username[0] = SplitData[0];
                MultiplayerMenuManager.instance.Usernames[0].text = MultiplayerManager.instance.Username[0];
                break;
            case 2:
                MultiplayerManager.instance.Username[0] = SplitData[0];
                MultiplayerManager.instance.Username[1] = SplitData[1];
                MultiplayerMenuManager.instance.Usernames[0].text = MultiplayerManager.instance.Username[0];
                MultiplayerMenuManager.instance.Usernames[1].text = MultiplayerManager.instance.Username[1];
                break;
            case 3:
                MultiplayerManager.instance.Username[0] = SplitData[0];
                MultiplayerManager.instance.Username[1] = SplitData[1];
                MultiplayerManager.instance.Username[2] = SplitData[2];
                MultiplayerMenuManager.instance.Usernames[0].text = MultiplayerManager.instance.Username[0];
                MultiplayerMenuManager.instance.Usernames[1].text = MultiplayerManager.instance.Username[1];
                MultiplayerMenuManager.instance.Usernames[2].text = MultiplayerManager.instance.Username[2];
                break;
            case 4:
                MultiplayerManager.instance.Username[0] = SplitData[0];
                MultiplayerManager.instance.Username[1] = SplitData[1];
                MultiplayerManager.instance.Username[2] = SplitData[2];
                MultiplayerManager.instance.Username[3] = SplitData[3];
                MultiplayerMenuManager.instance.Usernames[0].text = MultiplayerManager.instance.Username[0];
                MultiplayerMenuManager.instance.Usernames[1].text = MultiplayerManager.instance.Username[1];
                MultiplayerMenuManager.instance.Usernames[2].text = MultiplayerManager.instance.Username[2];
                MultiplayerMenuManager.instance.Usernames[3].text = MultiplayerManager.instance.Username[3];
                break;
        }
    }
    public static void ReadyOrNot(Packet _packet) 
    {
        bool[] ReadyRNot = new bool[_packet.ReadInt()];
        for (int i = 0; i < ReadyRNot.Length; i++)
        {
            ReadyRNot[i] = _packet.ReadBool();
            MultiplayerManager.instance.IsReady[i] = ReadyRNot[i];
        }
        MultiplayerMenuManager.instance.UpdateReadUpToggle(ReadyRNot.Length);
    }
    public static void LobbyisReady(Packet _packet) 
    {
        bool isLobbyReady = _packet.ReadBool();
        if (isLobbyReady) 
        {
            ClientSend.ClientNeedsPlayer();
        }
    }
    public static void RecievedWeaponValue(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        int NewWeaponValue = _packet.ReadInt();
        ClientManager.instance.NewWeaponValueRecevied(_id, NewWeaponValue);
    }
    public static void ServerShootingRecevied(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        bool IsFiring = _packet.ReadBool();
        ClientManager.instance.ShootingServerRecevied(_id, IsFiring);
    }
    public static void SpawnGroundEnemy(Packet _packet) 
    {
        Quaternion Rotation = _packet.ReadQuaternion();
        Vector3 Position = _packet.ReadVector3();
        int randomvalue = _packet.ReadInt();
        MultiplayerManager.instance.SM.clientEnemyManager.SpawnGroundEnemy(Rotation, Position , randomvalue);
    }
    public static void SpawnNormalEnemy(Packet _packet)
    {
        Quaternion Rotation = _packet.ReadQuaternion();
        Vector3 Position = _packet.ReadVector3();
        int randomvalue = _packet.ReadInt();
        MultiplayerManager.instance.SM.clientEnemyManager.SpawnNormalEnemy(Rotation, Position, randomvalue);
    }
    public static void ReceviedEnemyTransform(Packet _packet)
    {
        int ArrayLength = _packet.ReadInt();
        Quaternion[] NewRotation = new Quaternion[ArrayLength];
        Vector3[] NewPosition = new Vector3[ArrayLength];
        for (int i = 0; i < NewRotation.Length; i++)
        {
            NewRotation[i] = _packet.ReadQuaternion();
        }
        for (int i = 0; i < NewPosition.Length; i++)
        {
            NewPosition[i] = _packet.ReadVector3();
        }
        MultiplayerManager.instance.SM.clientEnemyManager.UpdateEnemyTransforms(NewRotation, NewPosition);
    }
    public static void EnemyDamageRecevied(Packet _packet)
    {
        int ArrayLength = _packet.ReadInt();
        int NewHealth = _packet.ReadInt();

        MultiplayerManager.instance.SM.clientEnemyManager.EnemyDamaged(ArrayLength, NewHealth);
    }
    public static void EnemyAttackRecevied(Packet _packet) 
    {
        int ArrayLength = _packet.ReadInt();

        MultiplayerManager.instance.SM.clientEnemyManager.EnemyAttackRecevied(ArrayLength);
    }
    public static void EnemyHitPlayer (Packet _packet) 
    {
        int _id = _packet.ReadInt();
        float NewHealth = _packet.ReadFloat();

        GameManager.players[_id].Health = NewHealth;
        GameManager.players[_id].DamageTaken();
    }
    public static void PlayerRevived(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.players[_id].PlayerAnimator.SetBool("CanRevive", true);
        GameManager.players[_id].isDown = false;
        GameManager.players[_id].CoinAmmount = GameManager.players[_id].CoinAmmount - 1;
        ClientGameMenu._clientGameMenu.UpdateCoinCount(_id);
        ClientGameMenu._clientGameMenu.UpdateHealth(_id, GameManager.players[_id].startingHealth);
        if (GameManager.players[_id].IsClient)
        {
            ClientGameMenu._clientGameMenu.ReviveGameObject.SetActive(false);
        }
    }
    public static void DropDropped(Packet _packet) 
    {
        int DropValue = _packet.ReadInt();
        Vector3 DropPosition = _packet.ReadVector3();
        bool DropStatus = _packet.ReadBool();
        if (DropStatus) 
        {
            ClientDropManager._clientDropManager.SpawnPrefab(DropValue, DropPosition);
        }
        else 
        {
            ClientDropManager._clientDropManager.RemoveDrop(DropValue);
        }
    }
    public static void DropData(Packet _packet)
    {
        int DropValue = _packet.ReadInt();
        bool DropStatus = _packet.ReadBool();
        if (DropStatus)
        {
            ClientDropManager._clientDropManager.DropsPickedUpOn(DropValue);
        }
        else 
        {
            ClientDropManager._clientDropManager.DropsPickedUpOff(DropValue);
        }
    }
    public static void PlayerDisconnected(Packet _Packet) 
    {
        int DisconnectedClient = _Packet.ReadInt();
        GameObject[] ConnectedPlayers;
        ConnectedPlayers = GameObject.FindGameObjectsWithTag("Player");
        _PlayerManager[] ConnectedPlayersClass;
        ConnectedPlayersClass = new _PlayerManager[ConnectedPlayers.Length];
        Debug.Log("Client" + DisconnectedClient + " Has Disconnected") ;
        for (int i = 0; i < ConnectedPlayers.Length; i++)
        {
            ConnectedPlayersClass[i] = ConnectedPlayers[i].GetComponent<_PlayerManager>();
        }
        for (int i = 0; i < ConnectedPlayers.Length; i++)
        {
            if (ConnectedPlayersClass[i].id == DisconnectedClient + 1) 
            {
                Debug.Log("FoundDisconnectedPlayer");
                Destroy(ConnectedPlayers[i]);
            }
        }
    }
    public static void NewRound(Packet _packet) 
    {
        int RoundValue = _packet.ReadInt();
        RoundManager._roundManager.ClientNewRound(RoundValue);
    }
    public static void NewScore(Packet _packet)
    {
        int PlayerId = _packet.ReadInt();
        int NewScore = _packet.ReadInt();
        ClientGameMenu._clientGameMenu.UpdateScore(PlayerId, NewScore);
    }
    public static void RecevieNewPerkValue(Packet _packet) 
    {
        int NewPerkValue = _packet.ReadInt();
        ClientPerkManager._clientPerkManager.NewPerkValueRecived(NewPerkValue);
    }
    public static void RecevieNewPerkData(Packet _packet)
    {
        bool PerkStatus = _packet.ReadBool();
        string PerkMessage = _packet.ReadString();
        ClientPerkManager._clientPerkManager.TextToggle(PerkStatus, PerkMessage);
    }
    public static void PerkBaught(Packet _packet)
    {
        int id = _packet.ReadInt();
        int PerkValue = _packet.ReadInt();
        ClientPerkManager._clientPerkManager.PlayerBaughtPerk(id,PerkValue);
    }
    public static void UpdateScore(Packet _packet)
    {
        int id = _packet.ReadInt();
        int NewScore = _packet.ReadInt();
        ClientGameMenu._clientGameMenu.UpdateScoreAgain(id, NewScore);
    }
    public static void GameEnded(Packet _packet) 
    {
        bool ended = _packet.ReadBool();
        DeathScreenManager._deathScreenManager.ShowMultiplayerHighScore();
    }
}
