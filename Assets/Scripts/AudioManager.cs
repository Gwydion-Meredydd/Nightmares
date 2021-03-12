using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    [Header("Enemy Noises")]
    int RandomEnemyNoiseSelectValue;
    int RandomEnemeyValue;
    public AudioClip[] EnemyRandomNoises;
    bool EnemyNoiseCooldownControl;
    [Range(1, 5)]
    public int EnemyNoiseCooldown;
    [Space]
    public AudioClip[] EnemyHitMarkers;
    [Space]
    public AudioClip[] EnemyAttacking;
    [Header("Weapon Noises")]
    int AssaultRifleAudioValue;
    public AudioClip[] AssaultRifle;
    public AudioClip[] MiniGun;
    int FlameThrowerAudioValue;
    [Range(0, 2)]
    public float FlameThrowerCooldownTime;
    bool FlameThrowerCooldownControl;
    [Range(0, 2)]
    public float OverallFlameThrowerCooldownTime;
    bool OverallFlameThrowerCooldownControl;
    public AudioClip[] FlameThrower;
    public AudioClip[] Shotgun;
    int ShotGunAudioValue;
    public AudioClip[] SideArm;
    int SideArmAudioValue;
    [Space]
    [Header("Player")]
    [Range(0.1f, 3)]
    public float FootstepDelay;
    bool Footstepping;
    public AudioClip[] FootstepDirt;
    public AudioClip[] FootstepStone;
    public AudioClip[] FootstepWood;
    public AudioClip[] FootstepMetal;
    [Space]
    public AudioClip[] PlayerHurtMale;
    public AudioClip[] PlayerHurtFemale;
    [Space]
    public AudioClip PerkDrinking;
    [Space]
    public AudioSource GameSFXAudioSource;
    [Space]
    [Header("Misc")]
    public AudioClip MenuSFX;
    public AudioClip NewRound;
    [Header("Music")]
    public GameObject MainMenuMusic;
    
    void Update()
    {
        if (SM.GameScript.InGame)
        {
            if (!SM.GameScript.Paused)
            {
                if (SM.EnemyScript.ActiveEnemies.Count > 0)
                {
                    EnemyRandomAudio();
                }
                if (!Footstepping) 
                {
                    Footstepping = true;
                    StartCoroutine(FootstepTiming());
                }
            }
        }
    }
    public void PlayMenuSFX()
    {
        GameSFXAudioSource.PlayOneShot(MenuSFX);
    }
    void EnemyRandomAudio()
    {
        RandomEnemyNoiseSelectValue = Random.Range(0, EnemyRandomNoises.Length);
        RandomEnemeyValue = Random.Range(0, SM.EnemyScript.ActiveEnemies.Count);
        if (!SM.EnemyScript.ActiveEnemiesAudioSources[RandomEnemeyValue].isPlaying && !EnemyNoiseCooldownControl)
        {
            SM.EnemyScript.ActiveEnemiesAudioSources[RandomEnemeyValue].clip = EnemyRandomNoises[RandomEnemyNoiseSelectValue];
            SM.EnemyScript.ActiveEnemiesAudioSources[RandomEnemeyValue].Play();
            StartCoroutine(EnemyNoiseCooldownTime());
        }
    }
    public void WeaponAudio() 
    {
        switch (SM.PlayerScript.WeaponValue)
        {
            case 1:
                SM.PlayerScript.WeaponAudioSource.PlayOneShot(AssaultRifle[AssaultRifleAudioValue]);
                AssaultRifleAudioValue = AssaultRifleAudioValue + 1;
                if (AssaultRifleAudioValue == 3)
                {
                    AssaultRifleAudioValue = 0;
                }
                break;
            case 2:
                if (Input.GetMouseButtonDown(0))
                {
                    SM.PlayerScript.WeaponAudioSource.PlayOneShot(MiniGun[0]);
                }
                SM.PlayerScript.WeaponAudioSource.PlayOneShot(MiniGun[2]);
                SM.PlayerScript.WeaponAudioSource.PlayOneShot(MiniGun[1]);
                break;
            case 3:
                if (Input.GetMouseButtonDown(0))
                {
                    SM.PlayerScript.WeaponAudioSource.PlayOneShot(FlameThrower[FlameThrowerAudioValue]);
                }
                if (!OverallFlameThrowerCooldownControl)
                {
                    //SM.PlayerScript.FlameThrowerAudioSource.PlayOneShot(FlameThrower[FlameThrowerAudioValue]);
                    if (!FlameThrowerCooldownControl)
                    {
                        SM.PlayerScript.WeaponAudioSource.PlayOneShot(FlameThrower[2]);
                    }
                    StartCoroutine(OverallFlameThrowerTime());
                    StartCoroutine(FlameThrowerCooldown());
                }
                FlameThrowerAudioValue = FlameThrowerAudioValue + 1;
                if (FlameThrowerAudioValue == 2)
                {
                    FlameThrowerAudioValue = 0;
                }
                break;
            case 4:
                SM.PlayerScript.WeaponAudioSource.PlayOneShot(Shotgun[ShotGunAudioValue]);
                ShotGunAudioValue = ShotGunAudioValue + 1;
                if (ShotGunAudioValue == 3)
                {
                    ShotGunAudioValue = 0;
                }
                break;
            case 5:
                SM.PlayerScript.WeaponAudioSource.PlayOneShot(SideArm[SideArmAudioValue]);
                SideArmAudioValue = SideArmAudioValue + 1;
                if (SideArmAudioValue == 3)
                {
                    SideArmAudioValue = 0;
                }
                break;
        }
    }
    IEnumerator FootstepTiming()
    {
        yield return new WaitForSeconds(FootstepDelay);
        if (SM.PlayerScript.Moving) 
        {
            switch (SM.PlayerScript.CurrentFootStepValue) 
            {
                case 0:
                    SM.PlayerScript.PlayerAudioSource.PlayOneShot(FootstepDirt[(Random.Range(0, 3))]);
                    break;
                case 1:
                    SM.PlayerScript.PlayerAudioSource.PlayOneShot(FootstepStone[(Random.Range(0, 3))]);
                    break;
                case 2:
                    SM.PlayerScript.PlayerAudioSource.PlayOneShot(FootstepWood[(Random.Range(0, 3))]);
                    break;
                case 3:
                    SM.PlayerScript.PlayerAudioSource.PlayOneShot(FootstepMetal[(Random.Range(0, 3))]);
                    break;
            }
        }
        Footstepping = false;
    }
    IEnumerator OverallFlameThrowerTime()
    {
        OverallFlameThrowerCooldownControl = true;
        yield return new WaitForSeconds(OverallFlameThrowerCooldownTime);
        OverallFlameThrowerCooldownControl = false;
    }
    IEnumerator FlameThrowerCooldown() 
    {
        FlameThrowerCooldownControl = true;
        yield return new WaitForSeconds(FlameThrowerCooldownTime);
        FlameThrowerCooldownControl = false;
    }
    IEnumerator EnemyNoiseCooldownTime()
    {
        EnemyNoiseCooldownControl = true;
        yield return new WaitForSeconds(EnemyNoiseCooldown);
        EnemyNoiseCooldownControl = false;
    }
}
