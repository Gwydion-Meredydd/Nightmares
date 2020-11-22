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
    [Header("Weapon Noises")]
    int AssaultRifleAudioValue;
    public AudioClip[] AssaultRifle;
    bool MiniGunStartupSound;
    public AudioClip[] MiniGun;
    int FlameThrowerAudioValue;
    [Range(0, 1)]
    public float FlameThrowerCooldownTime;
    bool FlameThrowerCooldownControl;
    public AudioClip[] FlameThrower;
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
            }
        }
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
                SM.PlayerScript.AssaultRifleAudioSource.PlayOneShot(AssaultRifle[AssaultRifleAudioValue]);
                AssaultRifleAudioValue = AssaultRifleAudioValue + 1;
                if (AssaultRifleAudioValue == 3)
                {
                    AssaultRifleAudioValue = 0;
                }
                break;
            case 2:
                if (Input.GetMouseButtonDown(0))
                {
                    SM.PlayerScript.MininGunAudioSource.PlayOneShot(MiniGun[0]);
                }
                SM.PlayerScript.MininGunAudioSource.PlayOneShot(MiniGun[2]);
                SM.PlayerScript.MininGunAudioSource.PlayOneShot(MiniGun[1]);
                break;
            case 3:
                if (Input.GetMouseButtonDown(0))
                {
                    SM.PlayerScript.FlameThrowerAudioSource.PlayOneShot(FlameThrower[FlameThrowerAudioValue]);
                }
                if (!FlameThrowerCooldownControl)
                {
                    SM.PlayerScript.FlameThrowerAudioSource.PlayOneShot(FlameThrower[2]);
                    StartCoroutine(FlameThrowerCooldown());
                }
                FlameThrowerAudioValue = FlameThrowerAudioValue + 1;
                if (FlameThrowerAudioValue == 2)
                {
                    FlameThrowerAudioValue = 0;
                }
                break;
        }
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
