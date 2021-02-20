using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    public static _GameManager instance;

    public static Dictionary<int, _PlayerManager> players = new Dictionary<int, _PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destryoing Object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == ClientManager.instance.myID)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else 
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }
        Debug.Log(_player);
        _player.GetComponent<_PlayerManager>().id = _id;
        _player.GetComponent<_PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<_PlayerManager>());
    }
}
