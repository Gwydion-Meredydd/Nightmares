﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PerksManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    public bool Testing;
    [Space]
    public int PerkValue;
    public int oldPerkValue;
    [Space]
    public Transform PerkSpawnPoint;
    public Quaternion PerkSpawnQuaternion;
    public Vector3 PerkSpawnVector;
    public GameObject[] PerkMachine;
    public Animator[] PerkMachineAnimator;
    public bool[] PerkBaught;
    [Space]
    public float PlayerDistance;
    public bool CanPurchase;
    public bool PerkMachineChange;
    [Space]
    [Header("Perk Costs")]
    public int CurrentCostOfPerk;
    [Space]
    public int HealthPerkCost;
    public int IncreasedPointsPerkCost;
    public int SpeedPerkCost;
    public int DamagePerkCost;
    void Update()
    {
        if (Testing == true) 
        {
            Testing = false;
            NewRound();
        }
        if (SM.GameScript.InGame == true)
        {
            if (SM.GameScript.Paused == false)
            {
                DistanceCalculation();
            }
        }
    }
    public void DistanceCalculation()
    {
        PlayerDistance = Vector3.Distance(SM.PlayerScript.Player.transform.position, PerkSpawnVector);
        if (PlayerDistance < 4)
        {
            CanPurchase = true;
        }
        else
        {
            CanPurchase = false;
        }
    }
    public void NewRound()
    {
        PerkMachineAnimator[PerkValue].SetBool("End",true);
        oldPerkValue = PerkValue;
        PerkValue = Random.Range(0, 4);
        if (PerkValue != oldPerkValue)
        {
            StartCoroutine(PerkAnimationTiming());
        }
        else
        {
            NewRound();
        }
        Debug.Log(PerkValue);
    }
    public void PlayerPerkBaught() 
    {
        switch (PerkValue)
        {
            case 0:
                SM.PlayerScript.StartingHealth = 200;
                SM.PlayerScript.Health = SM.PlayerScript.StartingHealth;
                break;
            case 1:
                SM.PlayerScript.ARDamage = SM.PlayerScript.ARDamage * 2;
                SM.PlayerScript.MGDamage = SM.PlayerScript.MGDamage * 2;
                SM.PlayerScript.FTDamage = SM.PlayerScript.FTDamage * 2;
                SM.PlayerScript.WeaponSwitch();
                break;
            case 2:
                SM.PlayerScript.MovingSpeed = SM.PlayerScript.MovingSpeed * 1.5F;
                break;
            case 3:
                SM.EnemyScript.PointsDamage = SM.EnemyScript.PointsDamage * 2;
                SM.EnemyScript.PointsKill = SM.EnemyScript.PointsKill * 2;
                break;
        }
    }
    IEnumerator PerkAnimationTiming()
    {
        PerkMachineChange = true;
        yield return new WaitForSecondsRealtime(0.8f);
        switch (PerkValue)
        {
            case 0:
                PerkMachine[1].SetActive(false);
                PerkMachine[2].SetActive(false);
                PerkMachine[3].SetActive(false);
                break;
            case 1:
                PerkMachine[0].SetActive(false);
                PerkMachine[2].SetActive(false);
                PerkMachine[3].SetActive(false);
                break;
            case 2:
                PerkMachine[0].SetActive(false);
                PerkMachine[1].SetActive(false);
                PerkMachine[3].SetActive(false);
                break;
            case 3:
                PerkMachine[0].SetActive(false);
                PerkMachine[1].SetActive(false);
                PerkMachine[2].SetActive(false);
                break;
        }
        PerkMachineAnimator[oldPerkValue].SetBool("End", false);
        switch (PerkValue)
        {
            case 0:
                PerkMachine[0].SetActive(true);
                CurrentCostOfPerk = HealthPerkCost;
                break;
            case 1:
                PerkMachine[1].SetActive(true);
                CurrentCostOfPerk = DamagePerkCost;
                break;
            case 2:
                PerkMachine[2].SetActive(true);
                CurrentCostOfPerk = SpeedPerkCost;
                break;
            case 3:
                PerkMachine[3].SetActive(true);
                CurrentCostOfPerk = IncreasedPointsPerkCost;
                break;
        }
        PerkMachineAnimator[PerkValue].SetBool("Start", true);
        yield return new WaitForSecondsRealtime(0.25f);
        PerkMachineAnimator[PerkValue].SetBool("Start", false);
        PerkMachineChange = false;
    }
}
