using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPerkManager : MonoBehaviour
{
    [HideInInspector]
    public static ClientPerkManager _clientPerkManager;
    [HideInInspector]
    public ClientPerkManager RefClientPerkManager;


    public Transform SpawnPoint;

    public GameObject SpawnedPerk;
    public Animator CurrentAnimaitor;
    public GameObject[] PerkPrefabs;

    private void Start()
    {
        RefClientPerkManager = this;
        _clientPerkManager = RefClientPerkManager;
    }

    public void NewPerkValueRecived(int NewValue)
    {
        if (SpawnPoint == null)
        {
            StartCoroutine(LevelSpawnDelay(NewValue));
        }
        else
        {

            StartCoroutine(PerkAnimationTiming(NewValue));
        }
    }
    IEnumerator LevelSpawnDelay(int NewValue)
    {
        yield return new WaitForSecondsRealtime(5f);
        GameObject PerkSpawnPointGO = GameObject.FindGameObjectWithTag("PerkSpawnPoint");

        SpawnPoint = PerkSpawnPointGO.transform;
        StartCoroutine(PerkAnimationTiming(NewValue));
    }
    public void TextToggle(bool Status, string Message) 
    {
        if (Status) 
        {
            ClientGameMenu._clientGameMenu.PerkText.gameObject.SetActive(true);
            ClientGameMenu._clientGameMenu.PerkText.text = Message; 
        }
        else 
        {
            ClientGameMenu._clientGameMenu.PerkText.gameObject.SetActive(false);
            ClientGameMenu._clientGameMenu.PerkText.text = "";
        }
    }
    IEnumerator PerkAnimationTiming(int NewValue)
    {
        yield return new WaitForSecondsRealtime(0.8f);
        if (CurrentAnimaitor != null)
        {
            CurrentAnimaitor.SetBool("End", false);
            yield return new WaitForSecondsRealtime(0.8f);
        }
        if (SpawnedPerk != null)
        {
            Destroy(SpawnedPerk);
        }
        SpawnedPerk = Instantiate(PerkPrefabs[NewValue], SpawnPoint.position, SpawnPoint.rotation);
        CurrentAnimaitor = SpawnedPerk.GetComponent<Animator>();
        CurrentAnimaitor.SetBool("Start", true);
        yield return new WaitForSecondsRealtime(0.25f);
        CurrentAnimaitor.SetBool("Start", false);
    }
    public void PlayerBaughtPerk(int id,int PerkValue) 
    {
        GameManager.players[id].PlayerAudioSource.PlayOneShot(AudioManager._audioManager.PerkDrinking);
        StartCoroutine(DrinkingPerkTime(id));
        ClientGameMenu._clientGameMenu.PerkText.text = "";
        switch (PerkValue) 
        {
            case 0:
                GameManager.players[id].startingHealth = 200;
                GameManager.players[id].Health= 200;
                ClientGameMenu._clientGameMenu.UpdateHealth(id, 200);
                break;
        }
    }
    IEnumerator DrinkingPerkTime(int id) 
    {
        GameManager.players[id].PlayerAnimator.SetBool("Perk", true);
        yield return new WaitForSecondsRealtime(0.1f);
        GameManager.players[id].PlayerAnimator.SetBool("Perk", false);
    }
}
