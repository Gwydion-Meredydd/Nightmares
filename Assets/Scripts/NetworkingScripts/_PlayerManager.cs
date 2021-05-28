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
    public float startingHealth;
    public float Health;
    public bool isDown;
    public int CoinAmmount;
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
    public Vector3 CurrentPosition;
    public Vector3 LastPosition;
    public float PlayerRotation;
    public bool FacingUp, FacingDown, FacingLeft, FacingRight;
    private Vector3 MovementDirectionValue;
    public int lives = 3;
    public Transform OcclusionPoint;
    private void Start()
    {
        if (!IsClient)
        {
            StartCoroutine(NonClientWalkClock());
        }
        else 
        {
            GameManager.instance.SM.CameraScript.OcclusionPoint = OcclusionPoint;
        }
    }
    private void Update()
    {
        if (IsClient)
        {
            PlayerRotationalCalculation();
            PlayerRoationAnimationCalculation();
        }
    }
    public void PlayerRoationAnimationCalculation()
    {
        //Checks which bools are active and sets the animator floats to the correct values
        //some needed inverting , and some needed to fully switched (horizontal and verticle)
        if (FacingUp)
        {
            PlayerAnimator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"), 0.1f, Time.deltaTime);
            PlayerAnimator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"), 0.1f, Time.deltaTime);
        }
        if (FacingDown)
        {
            PlayerAnimator.SetFloat("Horizontal", -Input.GetAxisRaw("Horizontal"), 0.1f, Time.deltaTime);
            PlayerAnimator.SetFloat("Vertical", -Input.GetAxisRaw("Vertical"), 0.1f, Time.deltaTime);
        }
        if (FacingLeft)
        {
            PlayerAnimator.SetFloat("Vertical", -Input.GetAxisRaw("Horizontal"), 0.1f, Time.deltaTime);
            PlayerAnimator.SetFloat("Horizontal", -Input.GetAxisRaw("Vertical"), 0.1f, Time.deltaTime);
        }
        if (FacingRight)
        {
            PlayerAnimator.SetFloat("Vertical", Input.GetAxisRaw("Horizontal"), 0.1f, Time.deltaTime);
            PlayerAnimator.SetFloat("Horizontal", Input.GetAxisRaw("Vertical"), 0.1f, Time.deltaTime);
        }
        MovementDirectionValue = new Vector3(Input.GetAxisRaw("Horizontal"), -1f, Input.GetAxisRaw("Vertical"));
        if (MovementDirectionValue.x != 0 || MovementDirectionValue.z != 0)//if the player is moving
        {
            PlayerAnimator.SetBool("Walk", true);
        }
        else
        {
            PlayerAnimator.SetBool("Walk", false);
        }
    }
    public void PlayerRotationalCalculation()
    {
        //gets the y value as an eular angle
        //checks the threshold of player rotation that counts as either left, right ,down
        //else if statments are used so that only one can be called at a time
        //up is left to last so it can be else stamtnet since eular angles goes to 359.99.. then 0 so if its not any other value its defaulted as up.
        if (this != null)
        {
            PlayerRotation = this.transform.eulerAngles.y;
        }
        if (PlayerRotation > 45 && PlayerRotation < 135)
        {
            FacingUp = false;
            FacingDown = false;
            FacingLeft = false;
            FacingRight = true;
        }
        else if (PlayerRotation > 135 && PlayerRotation < 225)
        {
            FacingUp = false;
            FacingDown = true;
            FacingLeft = false;
            FacingRight = false;
        }
        else if (PlayerRotation > 225 && PlayerRotation < 315)
        {
            FacingUp = false;
            FacingDown = false;
            FacingLeft = true;
            FacingRight = false;
        }
        else
        {
            FacingUp = true;
            FacingDown = false;
            FacingLeft = false;
            FacingRight = false;
        }
    }
    IEnumerator NonClientWalkClock()
    {
        yield return new WaitForSeconds(0.05f);
        CurrentPosition = this.gameObject.transform.position;
        if (CurrentPosition == LastPosition)
        {
            PlayerAnimator.SetBool("Walk", false);
        }
        else
        {
            PlayerAnimator.SetFloat("Vertical", 1f);
            PlayerAnimator.SetBool("Walk", true);
        }
        LastPosition = CurrentPosition;
        Debug.Log("WalkClock");
        StartCoroutine(NonClientWalkClock());
    }

    public void WeaponChange(int NewWeaponValue) 
    {
        if (PlayerAnimator != null)
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
        AudioManager._audioManager.ServerWeaponAudio(WeaponValue, this);
    }
    public void DamageTaken() 
    {
        PlayerAnimator.SetBool("Hurt", true);
        StartCoroutine(DamageCooldown());
        ClientGameMenu._clientGameMenu.UpdateHealth(id, Health);
        if (id == 3 || id == 4)
        {
            PlayerAudioSource.PlayOneShot(AudioManager._audioManager.PlayerHurtMale[(Random.Range(0, 3))]);
        }
        if (id == 1 || id == 2)
        {
            PlayerAudioSource.PlayOneShot(AudioManager._audioManager.PlayerHurtFemale[(Random.Range(0, 3))]);
        }
        Debug.Log(Health);
        if (Health <= 0)
        {
            PlayerAnimator.SetBool("CanRevive", false);
            PlayerAnimator.Play("Death", 0);
            isDown = true;
            if (IsClient)
            {
                if (CoinAmmount > 0)
                {
                    ClientGameMenu._clientGameMenu.ReviveGameObject.SetActive(true);
                }
                if (CoinAmmount == 0) 
                {
                    ClientGameMenu._clientGameMenu.DeadGameObject.SetActive(true);
                }
            }
        }
    }
    IEnumerator DamageCooldown() 
    {
        yield return new WaitForSecondsRealtime(0.1f);
        PlayerAnimator.SetBool("Hurt", false);
    }
}
