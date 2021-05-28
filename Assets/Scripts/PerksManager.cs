using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PerksManager : MonoBehaviour
{
    public ScriptsManager SM;
    public bool Testing;
    [HideInInspector]
    public int PerkValue;
    [HideInInspector]
    public int oldPerkValue;
    [HideInInspector]
    public Transform PerkSpawnPoint;
    [HideInInspector]
    public Quaternion PerkSpawnQuaternion;
    [HideInInspector]
    public Vector3 PerkSpawnVector;
    [HideInInspector]
    public GameObject[] PerkMachine;
    [HideInInspector]
    public Animator[] PerkMachineAnimator;
    [HideInInspector]
    public bool[] PerkBaught;
    [Space]
    [HideInInspector]
    public float PlayerDistance;
    [HideInInspector]
    public bool CanPurchase;
    [HideInInspector]
    public bool PerkMachineChange;
    [HideInInspector]
    public int CurrentCostOfPerk;
    [Header("Perk Costs")]
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
            InDistance();
        }
        else
        {
            CanPurchase = false;
            SM.GameMenuScript.PerkText.text = "";
        }
    }
    public void InDistance() 
    {
        SM.GameMenuScript.PerkText.gameObject.SetActive(true);
        switch (PerkValue) 
        {
            case 0:
                if (!PerkBaught[PerkValue])
                {
                    SM.GameMenuScript.PerkText.text = "PRESS F TO PURCHASE PERK FOR " + HealthPerkCost;
                }
                else
                {
                    SM.GameMenuScript.PerkText.text = "";
                }
                break;
            case 1:
                if (!PerkBaught[PerkValue])
                {
                    SM.GameMenuScript.PerkText.text = "PRESS F TO PURCHASE PERK FOR " + DamagePerkCost;
                }
                else
                {
                    SM.GameMenuScript.PerkText.text = "";
                }
                break;
            case 2:
                if (!PerkBaught[PerkValue])
                {
                    SM.GameMenuScript.PerkText.text = "PRESS F TO PURCHASE PERK FOR " + SpeedPerkCost;
                }
                else
                {
                    SM.GameMenuScript.PerkText.text = "";
                }
                break;
            case 3:
                if (!PerkBaught[PerkValue])
                {
                    SM.GameMenuScript.PerkText.text = "PRESS F TO PURCHASE PERK FOR " + IncreasedPointsPerkCost;
                }
                else
                {
                    SM.GameMenuScript.PerkText.text = "";
                }
                break;
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
    }
    public void PlayerPerkBaught() 
    {
        SM.PlayerScript.PlayerAudioSource.PlayOneShot(SM.AudioScripts.PerkDrinking);
        switch (PerkValue)
        {
            case 0:
                SM.PlayerScript.StartingHealth = 200;
                SM.PlayerScript.Health = SM.PlayerScript.StartingHealth;
                SM.GameMenuScript.HealthMethod(SM.PlayerScript.Health);
                break;
            case 1:
                SM.PlayerScript.ARDamage = SM.PlayerScript.ARDamage * 2;
                SM.PlayerScript.MGDamage = SM.PlayerScript.MGDamage * 2;
                SM.PlayerScript.FTDamage = SM.PlayerScript.FTDamage * 2;
                SM.PlayerScript.SGDamage = SM.PlayerScript.SGDamage * 2;
                SM.PlayerScript.SADamage = SM.PlayerScript.SADamage * 2;
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
