using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    [HideInInspector]
    public GameObject[] Drops;
    [HideInInspector]
    public GameObject[] ActiveDropsArray;
    [HideInInspector]
    public List<GameObject> ActiveDrops;
    [Range(1, 60)]
    public int TimeDropIsActive;
    [HideInInspector]
    public List<bool> CountDownInitlised;
    [HideInInspector]
    public List<int> CountDown;
    [HideInInspector]
    public int CountDownOverValue;
    [HideInInspector]
    public bool CountDownOver;
    [HideInInspector]
    public int OldRandom;
    [HideInInspector]
    public int RandomValue;
    [HideInInspector]
    public int ClosestDrop;
    [HideInInspector]
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
    [HideInInspector]
    public int DoublePointTime;
    [HideInInspector]
    public int InstantKillTime;
    [HideInInspector]
    public int BlindEyeTime;
    [HideInInspector]
    public int FlameThrowerTime;
    [HideInInspector]
    public int MiniGunTime;
    [HideInInspector]
    public int HealthBoostTime;
    [Space]
    [HideInInspector]
    public bool DoublePointsIsActive;
    [HideInInspector]
    public bool InstantKillIsActive;
    [HideInInspector]
    public bool BlindEyeIsActive;
    [HideInInspector]
    public bool FlameThrowerIsActive;
    [HideInInspector]
    public bool MiniGunIsActive;

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
        
    }
    void DropTimerRemoval() 
    {
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
            PickupEffect();
            Destroy(ActiveDrops[ClosestDrop]);
            ActiveDrops.RemoveAt(ClosestDrop);
            CountDownInitlised.RemoveAt(ClosestDrop);
            CountDown.RemoveAt(ClosestDrop);
            ActiveDropsArray = new GameObject[0];
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
                int newHealth = SM.PlayerScript.Health + SM.PlayerScript.StartingHealth / 2;
                if (newHealth <= SM.PlayerScript.StartingHealth)
                {
                    SM.PlayerScript.Health = newHealth;
                }
                else 
                {
                    SM.PlayerScript.Health = SM.PlayerScript.StartingHealth;
                }
                break;
        }
    }
    void PlayerDistanceCalculations() 
    {
        int i = 0;
        foreach  (GameObject Drop in ActiveDrops)
        {
            float NewDistance = Vector3.Distance(Drop.transform.position, SM.PlayerScript.Player.transform.position);
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
            i = i + 1;
        }
    }
    void NewRandomValue() 
    {
        OldRandom = RandomValue;
        RandomValue = Random.Range(0, Drops.Length);
        if (RandomValue == 5)
        {
            Debug.Log("HealthDropeed");
            if (SM.PlayerScript.Health == SM.PlayerScript.StartingHealth) 
            {
                NewRandomValue();
            }
        }
        else if (RandomValue == 3 || RandomValue == 4)
        {
            if (SM.PlayerScript.WeaponHeldValue == 3 || SM.PlayerScript.WeaponHeldValue == 2)
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
        Instantiate(Drops[RandomValue], DeathPoint, new Quaternion(0,0,0,0));
        ActiveDropsArray = GameObject.FindGameObjectsWithTag("Drop");
        ActiveDrops = new List<GameObject>(0);
        foreach (GameObject Drop in ActiveDropsArray)
        {
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
                if(CountDown[i] <= 0) 
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
        while (DoublePointsIsActive)
        {
            SM.PointsScript.DoublePoints = true;
            DoublePointTime = DoublePointTime - 1;
            yield return new WaitForSeconds(1f);
            if(DoublePointTime <= 0) 
            {
                SM.PointsScript.DoublePoints = false;
                DoublePointsIsActive = false;
            }
        }
    }
    IEnumerator InstantKill()
    {
        InstantKillTime = StartingInstantKillTime;
        InstantKillIsActive = true;
        while (InstantKillIsActive)
        {
            SM.PlayerScript.CurrentDamage = SM.EnemyScript.StartingHealth;
            InstantKillTime = InstantKillTime - 1;
            yield return new WaitForSeconds(1f);
            if (InstantKillTime <= 0)
            {
                InstantKillIsActive = false;
                SM.PlayerScript.WeaponSwitch();
            }
        }
    }
    IEnumerator BlindEye()
    {
        BlindEyeTime = StartingBlindEyeTime;
        BlindEyeIsActive = true;
        while (BlindEyeIsActive)
        {
            SM.EnemyScript.IgnorePlayer = true;
            BlindEyeTime = BlindEyeTime - 1;
            yield return new WaitForSeconds(1f);
            if (BlindEyeTime <= 0)
            {
                SM.EnemyScript.IgnorePlayer = false;
                BlindEyeIsActive = false;
            }
        }
    }
    IEnumerator MiniGun()
    {
        MiniGunTime = StartingMiniGunTime;
        MiniGunIsActive = true;
        SM.PlayerScript.WeaponValue = 2;
        SM.PlayerScript.WeaponSwitch();
        while (MiniGunIsActive)
        {
            MiniGunTime = MiniGunTime - 1;
            yield return new WaitForSeconds(1f);
            if (MiniGunTime <= 0)
            {
                SM.PlayerScript.WeaponValue = 1;
                SM.PlayerScript.WeaponSwitch();
                MiniGunIsActive = false;
            }
        }
    }
    IEnumerator FlameThrower()
    {
        FlameThrowerTime = StartingFlameThrowerTime;
        FlameThrowerIsActive = true;
        SM.PlayerScript.WeaponValue = 3;
        SM.PlayerScript.WeaponSwitch();
        while (FlameThrowerIsActive)
        {
            FlameThrowerTime = FlameThrowerTime - 1;
            yield return new WaitForSeconds(1f);
            if (FlameThrowerTime <= 0)
            {
                SM.PlayerScript.WeaponValue = 1;
                SM.PlayerScript.WeaponSwitch();
                FlameThrowerIsActive = false;
            }
        }
    }
}
