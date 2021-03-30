using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientHandleManager : MonoBehaviour
{
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
}
