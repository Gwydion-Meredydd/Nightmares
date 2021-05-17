using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDropManager : MonoBehaviour
{
    public static ServerDropManager _serverDropManager;
    public  ServerDropManager RefrenceserverDropManager;
    [Space]
    public GameObject[] Drops;
    public GameObject[] ActiveDropsArray;
    public List<GameObject> ActiveDrops;
    public GameObject[] Players;
    public ServerPlayer[] serverPlayers;
    [Range(1, 60)]
    public int TimeDropIsActive;
    public List<bool> CountDownInitlised;
    public List<int> CountDown;
    public int CountDownOverValue;
    public bool CountDownOver;
    public int OldRandom;
    public int RandomValue;
    public int ClosestDrop;
    public float PlayerDistance;
    [Header("Drop Timing")]
    [Range(1, 60)]
    public int StartingDoublePointsTime;
    [Range(1, 60)]
    public int StartingInstantKillTime;
    [Range(1, 60)]
    public int StartingBlindEyeTime;
    [Range(1, 60)]
    public int StartingFlameThrowerTime;
    [Range(1, 60)]
    public int StartingMiniGunTime;
    [Space]
    public int DoublePointTime;
    public int InstantKillTime;
    public int BlindEyeTime;
    public int FlameThrowerTime;
    public int MiniGunTime;
    public int HealthBoostTime;
    [Space]
    public bool DoublePointsIsActive;
    public bool InstantKillIsActive;
    public bool BlindEyeIsActive;
    public bool FlameThrowerIsActive;
    public bool MiniGunIsActive;


    private void Start()
    {
        RefrenceserverDropManager = this;
        _serverDropManager = RefrenceserverDropManager;
    }

    void Update()
    {
        if (ActiveDrops.Count > 0)
        {
            if (CountDownOver == false)
            {
                PlayerDistanceCalculations();
                PickupDrop();
            }
            else
            {
                DropTimerRemoval();
            }
        }
        if (Players.Length == 0)
        {
            Players = GameObject.FindGameObjectsWithTag("Player");
        }
        if (serverPlayers.Length == 0)
        {
            if (Players.Length > 0)
            {
                serverPlayers = new ServerPlayer[Players.Length];
                for (int i = 0; i < Players.Length; i++)
                {
                    serverPlayers[i] = Players[i].GetComponent<ServerPlayer>(); 
                }
            }
        }

    }
    void DropTimerRemoval()
    {
        ServerSend.DropDropped(CountDownOverValue, new Vector3(0f,0f,0f), false);
        Destroy(ActiveDrops[CountDownOverValue]);
        ActiveDrops.RemoveAt(CountDownOverValue);
        CountDown.RemoveAt(CountDownOverValue);
        CountDownInitlised.RemoveAt(CountDownOverValue);

        CountDownOverValue = 0;
        CountDownOver = false;
    }
    void PickupDrop()
    {
        if (PlayerDistance < 0.5f)
        {
            ServerSend.DropDropped(ClosestDrop, ActiveDrops[ClosestDrop].transform.position, false);
            PickupEffect();
            Destroy(ActiveDrops[ClosestDrop]);
            ActiveDrops.RemoveAt(ClosestDrop);
            CountDownInitlised.RemoveAt(ClosestDrop);
            CountDown.RemoveAt(ClosestDrop);
            ActiveDropsArray = GameObject.FindGameObjectsWithTag("Drop");
            PlayerDistance = 0;
        }
    }
    void PickupEffect()
    {
        switch (ActiveDrops[ClosestDrop].layer)
        {
            case 10:
                if (DoublePointsIsActive == false)
                {
                    StartCoroutine(DoublePoints());
                }
                else
                {
                    DoublePointTime = StartingDoublePointsTime;
                }
                break;
            case 11:
                if (InstantKillIsActive == false)
                {
                    StartCoroutine(InstantKill());
                }
                else
                {
                    InstantKillTime = StartingInstantKillTime;
                }
                break;
            case 12:
                if (BlindEyeIsActive == false)
                {
                    StartCoroutine(BlindEye());
                }
                else
                {
                    BlindEyeTime = StartingBlindEyeTime;
                }
                break;
            case 13:
                if (MiniGunIsActive == false)
                {
                    StartCoroutine(MiniGun());
                }
                else
                {
                    MiniGunTime = StartingMiniGunTime;
                }
                break;
            case 14:
                if (FlameThrowerIsActive == false)
                {
                    StartCoroutine(FlameThrower());
                }
                else
                {
                    FlameThrowerTime = StartingFlameThrowerTime;
                }
                break;
            case 19:
                foreach (ServerPlayer sPlayer in serverPlayers)
                {
                    sPlayer.Health = sPlayer.CurrentMaxHealth;
                    ServerSend.DropData(5, true);
                }
                break;
        }
    }
    void PlayerDistanceCalculations()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
        foreach (GameObject Drop in ActiveDrops)
        {
            foreach (GameObject Player in Players)
            {
                float NewDistance = Vector3.Distance(Drop.transform.position, Player.transform.position);
                if (PlayerDistance == 0)
                {
                    PlayerDistance = NewDistance;
                }
                else
                {
                    if (PlayerDistance > NewDistance)
                    {
                        ClosestDrop = i;
                        PlayerDistance = NewDistance;
                    }
                }
            }
            i = i + 1;
        }
    }
    void NewRandomValue()
    {
        OldRandom = RandomValue;
        RandomValue = Random.Range(0, Drops.Length);
        if (RandomValue == 5)
        {
            bool checkifneedhealth = true;
            foreach (ServerPlayer sPlayer in serverPlayers)
            {
                if (sPlayer.Health != sPlayer.CurrentMaxHealth)
                {
                    checkifneedhealth = false;
                }
                if (checkifneedhealth)
                {
                    NewRandomValue();
                }
            }
        }
        else if (RandomValue == 3 || RandomValue == 4)
        {
            bool checkifneedGuns = true;
            foreach (ServerPlayer sPlayer in serverPlayers)
            {
                if (sPlayer.WeaponValue == 3 || sPlayer.WeaponValue == 2)
                {
                    checkifneedGuns = false;
                }
            }
            if (!checkifneedGuns)
            {
                NewRandomValue();
            }
        }
        else
        {
            if (OldRandom == RandomValue)
            {
                NewRandomValue();
            }
        }
    }
    public void DropRandom(Vector3 DeathPoint)
    {
        NewRandomValue();
        Debug.Log("Drop Random Called");
        Instantiate(Drops[RandomValue], DeathPoint, new Quaternion(0, 0, 0, 0));
        ActiveDropsArray = GameObject.FindGameObjectsWithTag("Drop");
        ActiveDrops = new List<GameObject>(0);
        foreach (GameObject Drop in ActiveDropsArray)
        {
            Debug.Log("Active Drop foreach");
            ActiveDrops.Add(Drop);
        }
        CountDown.Add(1);
        CountDownInitlised.Add(false);
        for (int i = 0; i < ActiveDrops.Count; i++)
        {
            if (CountDownInitlised[i] == false)
            {
                CountDown[i] = TimeDropIsActive;
                CountDownInitlised[i] = true;
            }
        }
        Debug.Log("ServerDropSending");
        ServerSend.DropDropped(RandomValue, DeathPoint,true);
        StartCoroutine(DropRemoval());
    }
    IEnumerator DropRemoval()
    {
        if (ActiveDrops.Count > 0)
        {
            for (int i = 0; i < ActiveDrops.Count; i++)
            {
                CountDown[i] = CountDown[i] - 1;
                yield return new WaitForSeconds(1f);
            }
            for (int i = 0; i < ActiveDrops.Count; i++)
            {
                if (CountDown[i] <= 0)
                {
                    CountDownOver = true;
                    CountDownOverValue = i;
                    break;
                }
            }
            StartCoroutine(DropRemoval());
        }
    }
    IEnumerator DoublePoints()
    {
        DoublePointTime = StartingDoublePointsTime;
        DoublePointsIsActive = true;
        ServerSend.DropData(0, true);
        while (DoublePointsIsActive)
        {
            ServerPoints._serverPoints.DoublePoints = true;
            DoublePointTime = DoublePointTime - 1;
            yield return new WaitForSeconds(1f);
            if (DoublePointTime <= 0)
            {
                ServerPoints._serverPoints.DoublePoints = false;
                ServerSend.DropData(0, false);
                DoublePointsIsActive = false;
            }
        }
    }
    IEnumerator InstantKill()
    {
        InstantKillTime = StartingInstantKillTime;
        InstantKillIsActive = true;
        ServerSend.DropData(1, true);
        while (InstantKillIsActive)
        {
            ServerEnemyManager._serverEnemyManager.CurrentDamage = ServerEnemyManager._serverEnemyManager.StartingHealth;
            InstantKillTime = InstantKillTime - 1;
            yield return new WaitForSeconds(1f);
            if (InstantKillTime <= 0)
            {
                ServerSend.DropData(1, false);

                InstantKillIsActive = false;
            }
        }
    }
    IEnumerator BlindEye()
    {
        BlindEyeTime = StartingBlindEyeTime;
        BlindEyeIsActive = true;
        ServerSend.DropData(2, true);
        while (BlindEyeIsActive)
        {
            ServerEnemyManager._serverEnemyManager.IgnorePlayer = true;
            ServerEnemyManager._serverEnemyManager.ChaseRandom();
            BlindEyeTime = BlindEyeTime - 1;
            yield return new WaitForSeconds(1f);
            if (BlindEyeTime <= 0)
            {
                ServerEnemyManager._serverEnemyManager.ChasePlayer();
                ServerEnemyManager._serverEnemyManager.IgnorePlayer = false;
                ServerSend.DropData(2, false);
                BlindEyeIsActive = false;
            }
        }
    }
    IEnumerator MiniGun()
    {
        MiniGunTime = StartingMiniGunTime;
        MiniGunIsActive = true;
        ServerSend.DropData(3, true);
        while (MiniGunIsActive)
        {
            foreach (ServerPlayer sPlayer in serverPlayers)
            {
                sPlayer.WeaponValue = 2;
                sPlayer.NewWeaponValue(sPlayer.id, sPlayer.WeaponValue);
            }
            MiniGunTime = MiniGunTime - 1;
            yield return new WaitForSeconds(1f);
            if (MiniGunTime <= 0)
            {
                foreach (ServerPlayer sPlayer in serverPlayers)
                {
                    sPlayer.WeaponValue = 1;
                    sPlayer.NewWeaponValue(sPlayer.id, sPlayer.WeaponValue);
                }
                ServerSend.DropData(3, false);
                MiniGunIsActive = false;
            }
        }
    }
    IEnumerator FlameThrower()
    {
        FlameThrowerTime = StartingFlameThrowerTime;
        FlameThrowerIsActive = true;
        ServerSend.DropData(4, true);
        while (FlameThrowerIsActive)
        {
            FlameThrowerTime = FlameThrowerTime - 1;
            foreach (ServerPlayer sPlayer in serverPlayers)
            {
                sPlayer.WeaponValue = 3;
                sPlayer.NewWeaponValue(sPlayer.id, sPlayer.WeaponValue);
            }
            yield return new WaitForSeconds(1f);
            if (FlameThrowerTime <= 0)
            {
                foreach (ServerPlayer sPlayer in serverPlayers)
                {
                    sPlayer.WeaponValue = 1;
                    sPlayer.NewWeaponValue(sPlayer.id, sPlayer.WeaponValue);
                }
                ServerSend.DropData(4, false);
                FlameThrowerIsActive = false;
            }
        }
    }
}
