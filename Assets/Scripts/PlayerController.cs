using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager Game_Manager;
    public CameraController CameraScript;
    [Header("Selected Player")]
    public GameObject Player;
    public Animator PlayerAnimator;
    public CharacterController PlayerCc;
    [Header("Movement Variables")]
    public float MovingSpeed;
    private Vector3 MovementDirectionValue;
    [Header("WeaponVariables")]
    public bool TestingSwitchWeapon;
    [Range(1, 3)]
    public int WeaponValue;
    public int WeaponHeldValue;
    public GameObject AutomaticRifleModel;
    public GameObject MiniGunModel;
    public GameObject FlameThrowerModel;
    public Transform ARShootPoint;
    public Transform MGShootPoint;
    public Transform FTShootPoint;
    public Transform ARCasePoint;
    public Transform MGCasePoint;
    public float AssaultRifleFiringTime;
    public float MiniGunFiringTime;
    public float FlameThrowerFiringTime;
    private bool Firing;
    [Space]
    public ParticleSystem ARBulletParticle;
    public ParticleSystem ARMuzzleFlash;
    public ParticleSystem ARBulletCasing;
    public ParticleSystem MGBulletParticle;
    public ParticleSystem MGMuzzleFlash;
    public ParticleSystem MGBulletCasing;
    public ParticleSystem FTFlame;
    public ParticleSystem FTHeatDistortion;
    [Space]
    public EnemyManager EnemyScript;
    public int CurrentDamage;
    public int ARDamage;
    public int MGDamage;
    public int FTDamage;
    public Ray RayCastRay;
    public RaycastHit RayCastHit;
    public GameObject HitTarget;
    public GameObject HitEnemy;


    //For camera punch effect
    [Range(0, 1)]
    public float CameraShootEffectValue;
    float TempPunchValue;
    void Update()
    {
        if (Game_Manager.InGame == true)
        {
            if (Game_Manager.Paused == false)
            {
                //Is called when game is unpased and game is currently active

                PlayerInputs();

                //Weapon Switch Whilst Testing
                if (TestingSwitchWeapon == true)
                {
                    if (WeaponHeldValue != WeaponValue)
                    {
                        WeaponHeldValue = WeaponValue;
                        WeaponSwitch();
                    }
                }
            }
        }
    }
    public void WeaponSwitch() 
    {
        switch (WeaponValue)
        {
            case 1:
                AutomaticRifleModel.SetActive(true);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(false);
                CurrentDamage = ARDamage;
                break;
            case 2:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(true);
                FlameThrowerModel.SetActive(false);
                CurrentDamage = MGDamage;
                break;
            case 3:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(true);
                CurrentDamage = FTDamage;
                break;
        }
    }
    public void RayCastMethod() 
    {
        switch (WeaponValue)
        {
            case 1:
                if (Physics.Raycast(ARShootPoint.position, ARShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        HitEnemy = RayCastHit.transform.gameObject;
                        EnemyScript.EnemyHited = HitEnemy;
                        if (EnemyScript.HealthCalculation == false)
                        {
                            EnemyScript.HealthManager();
                        }
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
            case 2:
                if (Physics.Raycast(MGShootPoint.position, MGShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        HitEnemy = RayCastHit.transform.gameObject;
                        EnemyScript.EnemyHited = HitEnemy;
                        if (EnemyScript.HealthCalculation == false)
                        {
                            EnemyScript.HealthManager();
                        }
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
            case 3:
                if (Physics.Raycast(FTShootPoint.position, FTShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        HitEnemy = RayCastHit.transform.gameObject;
                        EnemyScript.EnemyHited = HitEnemy;
                        if (EnemyScript.HealthCalculation == false)
                        {
                            EnemyScript.HealthManager();
                        }
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
        }
    }
    public void PlayerInputs()
    {
        //Methods for player based inputs

        MousePlayerRotation();
        CharacterMovement();
        if (Input.GetMouseButton(0)) 
        {
            StartCoroutine(ShootingMethod());
        }
    }
    #region Player Movement
    public void CharacterMovement()
    {
        //Moves the selected Character in a global position using the character controller
       
        MovementDirectionValue = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        PlayerCc.Move((MovementDirectionValue * MovingSpeed)* Time.deltaTime);
        AnimationIntilisation();
    }
    public void AnimationIntilisation()
    {
        if (MovementDirectionValue.x != 0 || MovementDirectionValue.z != 0) 
        {
            PlayerAnimator.SetBool("Walk",true);
        }
        else 
        {
            PlayerAnimator.SetBool("Walk", false);
        }
        if (Firing == true) 
        {
            PlayerAnimator.SetBool("Firing", true);
        }
        else if (!Input.GetMouseButton(0))
        {
         
            PlayerAnimator.SetBool("Firing", false);
        }
    }
    public void MousePlayerRotation()
    {
        //rotates the players body towards the mouses position

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            Player.transform.LookAt(new Vector3(hit.point.x, Player.transform.position.y, hit.point.z));
        }
    }
    #endregion
    IEnumerator ShootingMethod() 
    {
        if (Firing == false)
        {
            Firing = true;
            //Weapon value indicates which weapon is selected 1= auto rifle 2= mini gun 3= flamethrower , 2 & 3 are drops for limated time
            switch (WeaponValue)
            {
                case 1:
                    ARBulletParticle.Emit(1);
                    ARBulletCasing.Emit(1);
                    ARMuzzleFlash.Emit(1);
                    RayCastMethod();
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(AssaultRifleFiringTime);
                    CameraScript.yValue = TempPunchValue;
                    break;
                case 2:
                    MGBulletParticle.Emit(1);
                    MGBulletCasing.Emit(1);
                    MGMuzzleFlash.Emit(1);
                    RayCastMethod();
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(MiniGunFiringTime);
                    CameraScript.yValue = TempPunchValue;
                    break;
                case 3:
                    FTFlame.Emit(5);
                    FTHeatDistortion.Emit(1);
                    RayCastMethod();
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(FlameThrowerFiringTime);
                    CameraScript.yValue = TempPunchValue;
                    break;
            }
            Firing = false;
        }

    }
    public void CameraPunch() 
    {
        TempPunchValue = CameraScript.yValue;
        CameraScript.yValue -= CameraShootEffectValue;
    }
}
