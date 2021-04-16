using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerManager : MonoBehaviour
{
    public bool IsClient;
    public int id;
    public string username;
    public int WeaponValue;
    public int WeaponHeldValue;
    public GameObject AutomaticRifleModel;
    public GameObject MiniGunModel;
    public GameObject FlameThrowerModel;
    public GameObject ShotGunModel;
    public GameObject SideArmModel;
    public Transform ARShootPoint;
    public Transform MGShootPoint;
    public Transform FTShootPoint;
    public Transform[] SGShootPoint;
    public Transform SAShootPoint;
    public Transform ARCasePoint;
    public Transform MGCasePoint;
    public Transform SGCasePoint;
    public Transform SACasePoint;
    public AudioSource WeaponAudioSource;
    public AudioSource PlayerAudioSource;
    public ParticleSystem ARBulletParticle;
    public ParticleSystem ARMuzzleFlash;
    public ParticleSystem ARBulletCasing;
    public ParticleSystem MGBulletParticle;
    public ParticleSystem MGMuzzleFlash;
    public ParticleSystem MGBulletCasing;
    public ParticleSystem FTFlame;
    public ParticleSystem FTHeatDistortion;
    public ParticleSystem[] SGBulletParticle;
    public ParticleSystem SGMuzzleFlash;
    public ParticleSystem SGBulletCasing;
    public ParticleSystem SABulletParticle;
    public ParticleSystem SAMuzzleFlash;
    public ParticleSystem SABulletCasing;
    [Space]
    public Animator PlayerAnimator;

    private void Update()
    {
        
    }
    public void WeaponChange(int NewWeaponValue) 
    {
        switch (NewWeaponValue)
        {
            case 1:
                AutomaticRifleModel.SetActive(true);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(false);
                ShotGunModel.SetActive(false);
                SideArmModel.SetActive(false);
                PlayerAnimator.SetBool("Shotgun", false);
                PlayerAnimator.SetBool("SideArm", false);
                break;
            case 2:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(true);
                FlameThrowerModel.SetActive(false);
                ShotGunModel.SetActive(false);
                SideArmModel.SetActive(false);
                PlayerAnimator.SetBool("Shotgun", false);
                PlayerAnimator.SetBool("SideArm", false);
                break;
            case 3:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(true);
                ShotGunModel.SetActive(false);
                SideArmModel.SetActive(false);
                PlayerAnimator.SetBool("SideArm", false);
                PlayerAnimator.SetBool("Shotgun", false);
                break;
            case 4:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(false);
                ShotGunModel.SetActive(true);
                SideArmModel.SetActive(false);
                PlayerAnimator.SetBool("SideArm", false);
                PlayerAnimator.SetBool("Shotgun", true);
                break;
            case 5:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(false);
                ShotGunModel.SetActive(false);
                SideArmModel.SetActive(true);
                PlayerAnimator.SetBool("Shotgun", false);
                PlayerAnimator.SetBool("SideArm", true);
                break;
        }
        WeaponValue = NewWeaponValue;
    }
    public void ShootingServerRecevied(bool IsFiring) 
    {
        Shoot();
    }
    public void Shoot() 
    {
        switch (WeaponValue)
        {
            case 1:
                ARBulletParticle.Emit(1);
                ARBulletCasing.Emit(1);
                ARMuzzleFlash.Emit(1);
                break;
            case 2:
                MGBulletParticle.Emit(1);
                MGBulletCasing.Emit(1);
                MGMuzzleFlash.Emit(1);
                break;
            case 3:
                FTFlame.Emit(10);
                FTHeatDistortion.Emit(1);
                break;
            case 4:
                SGBulletParticle[0].Emit(1);
                SGBulletParticle[1].Emit(1);
                SGBulletParticle[2].Emit(1);
                SGBulletParticle[3].Emit(1);
                SGBulletParticle[4].Emit(1);
                SGBulletCasing.Emit(1);
                SGMuzzleFlash.Emit(1);
                break;
            case 5:
                SABulletParticle.Emit(1);
                SABulletCasing.Emit(1);
                SAMuzzleFlash.Emit(1);
                break;
        }
    }
}
