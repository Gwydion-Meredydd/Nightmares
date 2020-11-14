using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksManager : MonoBehaviour
{
    public bool Testing;
    [Space]
    public GameManager GameScript;
    public PlayerController PlayerScript;
    [Space]
    public int PerkValue;
    public int oldPerkValue;
    [Space]
    public Transform PerkSpawnPoint;
    public Quaternion PerkSpawnQuaternion;
    public Vector3 PerkSpawnVector;
    public GameObject[] PerkMachine;
    public Animator[] PerkMachineAnimator;
    [Space]
    public float PlayerDistance;
    public bool CanPurchase;
    public bool PerkMachineChange;
    void Update()
    {
        if (Testing == true) 
        {
            Testing = false;
            NewRound();
        }
        if (GameScript.InGame == true)
        {
            if (GameScript.Paused == false)
            {
                DistanceCalculation();
            }
        }
    }
    public void DistanceCalculation()
    {
        PlayerDistance = Vector3.Distance(PlayerScript.Player.transform.position, PerkSpawnVector);
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
                break;
            case 1:
                PerkMachine[1].SetActive(true);
                break;
            case 2:
                PerkMachine[2].SetActive(true);
                break;
            case 3:
                PerkMachine[3].SetActive(true);
                break;
        }
        PerkMachineAnimator[PerkValue].SetBool("Start", true);
        yield return new WaitForSecondsRealtime(0.25f);
        PerkMachineAnimator[PerkValue].SetBool("Start", false);
        PerkMachineChange = false;
    }
}
