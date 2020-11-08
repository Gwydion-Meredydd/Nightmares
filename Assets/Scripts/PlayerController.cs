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
    [Header("Health")]
    public int Health;
    public int StartingHealth;
    [Range(0, 5)]
    public float CameraDamageEffectValue;
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
    public bool Firing;
    public bool CanShoot;
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
    [Space]
    [Header("Animation Veriables")]
    public float PlayerRotation;
    public bool FacingUp, FacingDown, FacingLeft, FacingRight;


    [Space]
    [Header("Camera Effects")]
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
                if (Firing == true && CanShoot == false) 
                {
                    CanShoot = true;
                    RayCastMethod();
                }
            }
        }
    }
    public void WeaponSwitch() 
    {
        //switch function depending on weapon value
        //turns on/off the right/wrong weapon
        //sets the correct damage value for that wepon
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
        //swtich statment depending on the weapon value to make sure the raycast is shot from the correct point
        //if the raycast hits and enemy it calls the healthmanger method on the enemy script and sets enemy hitted to the raycast collison object in enemy script
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
        if (Input.GetMouseButton(0)) //checks if the player presses the left mouse button and calls the shooting method
        {
            StartCoroutine(ShootingMethod());
        }
    }
    #region Player Movement
    public void CharacterMovement()
    {
        //Moves the selected Character in a global position using the character controller
        //calls the animaton method once the movment has been applied to the character controller
        MovementDirectionValue = new Vector3(Input.GetAxisRaw("Horizontal"), -1f, Input.GetAxisRaw("Vertical"));
        PlayerCc.Move((MovementDirectionValue * MovingSpeed)* Time.deltaTime);
        AnimationIntilisation();
    }
    public void AnimationIntilisation()
    {
        //sets the correct bools (walk and shoot) in the animator , if the correct paramiters apply.
        if (MovementDirectionValue.x != 0 || MovementDirectionValue.z != 0)//if the player is moving
        {
            PlayerAnimator.SetBool("Walk", true);
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
        //methods that calculate the local rotation of the player and which way the legs need to walk
        //this is needed since the player rotates with mouse and the values change constantly 
        PlayerRotationalCalculation();
        PlayerRoationAnimationCalculation();
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
    }
    public void PlayerRotationalCalculation() 
    {
        //gets the y value as an eular angle
        //checks the threshold of player rotation that counts as either left, right ,down
        //else if statments are used so that only one can be called at a time
        //up is left to last so it can be else stamtnet since eular angles goes to 359.99.. then 0 so if its not any other value its defaulted as up.
        PlayerRotation = Player.transform.eulerAngles.y;
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
    public void MousePlayerRotation()
    {
        //rotates the players body towards the mouses position
        //uses raycast
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
        //shoot method that only works if the player isnt allready shooting
        //uses switch statment for each weapon 
        //uses emit system to play the bullet,case,and muzzle effect
        //calls the raycast method to check if a eneemy is hitted
        //does the camera effect
        //waits for the gun cooldown for each weapon
        //resets the camera effect
        //turns the firing bool off allowing the player to shoot again
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
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(AssaultRifleFiringTime);
                    CameraScript.yValue = TempPunchValue;
                    CanShoot = false;
                    break;
                case 2:
                    MGBulletParticle.Emit(1);
                    MGBulletCasing.Emit(1);
                    MGMuzzleFlash.Emit(1);
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(MiniGunFiringTime);
                    CameraScript.yValue = TempPunchValue;
                    CanShoot = false;
                    break;
                case 3:
                    FTFlame.Emit(10);
                    FTHeatDistortion.Emit(1);
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(FlameThrowerFiringTime);
                    CameraScript.yValue = TempPunchValue;
                    CanShoot = false;
                    break;
            }
            Firing = false;
        }

    }
    public void CameraDamage()
    {
        //method that changes the camera zoom to indicate the player has taken damage
        CameraScript.yValue -= CameraDamageEffectValue;
    }
    public void CameraPunch() 
    {
        //method that changes the zoom level of the camera to show that the player is firing
        TempPunchValue = CameraScript.yValue;
        CameraScript.yValue -= CameraShootEffectValue;
    }
}
