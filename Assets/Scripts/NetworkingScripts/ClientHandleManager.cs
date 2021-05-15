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
        Debug.Log($"Message from Server: {_msg}");
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
        Debug.Log(_username);
        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
        GameManager.instance.SM.LevelScript.SpawnMultiplayerLevel();
    }
    public static void PlayerPosition(Packet _packet) 
    {
        Debug.Log("ClientPlayerPos");
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        
        GameManager.players[_id].transform.position = Vector3.Lerp(GameManager.players[_id].transform.position, _position, Time.fixedDeltaTime * 10);
    }
    public static void PlayerRotation(Packet _packet)
    {
        Debug.Log("ClientPlayerRos");
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }
    public static void PlayerJoined(Packet _packet) 
    {
        string PacketData = _packet.ReadString().ToString();
        string[] SplitData = PacketData.Split(new char[] { '*' }, System.StringSplitOptions.RemoveEmptyEntries);
        MultiplayerManager.instance.Username = new string[SplitData.Length];
        for (int i = 0; i < SplitData.Length; i++)
        {
            Debug.Log(SplitData.Length);
            Debug.Log(SplitData[i]);
        }
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
        Debug.Log("Ready Toggle Received From Server...");
        bool[] ReadyRNot = new bool[_packet.ReadInt()];
        Debug.Log(ReadyRNot.Length);
        for (int i = 0; i < ReadyRNot.Length; i++)
        {
            ReadyRNot[i] = _packet.ReadBool();
            MultiplayerManager.instance.IsReady[i] = ReadyRNot[i];
        }
        MultiplayerMenuManager.instance.UpdateReadUpToggle(ReadyRNot.Length);
    }
    public static void LobbyisReady(Packet _packet) 
    {
        Debug.Log("Lobby is Ready from server...");
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
        Debug.Log("Recieved Weapon Value");
    }
    public static void ServerShootingRecevied(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        bool IsFiring = _packet.ReadBool();
        ClientManager.instance.ShootingServerRecevied(_id, IsFiring);
        Debug.Log("Recieved Shooting");
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
        int NewHealth = _packet.ReadInt();

        GameManager.players[_id].Health = NewHealth;
        GameManager.players[_id].DamageTaken();
        Debug.Log("Client " + _id + " Took Damage");
    }
    public static void PlayerRevived(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Debug.Log("Revive Player");
        GameManager.players[_id].PlayerAnimator.SetBool("CanRevive", true);
    }
}
