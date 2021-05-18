using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ScriptsManager SM;
 
    public GameObject Player;
    [HideInInspector]
    public Animator PlayerAnimator;
    [HideInInspector]
    public CharacterController PlayerCc;
    [Header("Health")]
    public float Health;
    public int StartingHealth;
    [Range(0, 5)]
    public float CameraDamageEffectValue;
    [Header("Movement Variables")]
    public float MovingSpeed;
    private Vector3 MovementDirectionValue;
    [Header("WeaponVariables")]
    public bool TestingSwitchWeapon;
    [Range(1, 5)]
    public int WeaponValue;
    [HideInInspector]
    public int WeaponHeldValue;
    [HideInInspector]
    public GameObject AutomaticRifleModel;
    [HideInInspector]
    public GameObject MiniGunModel;
    [HideInInspector]
    public GameObject FlameThrowerModel;
    [HideInInspector]
    public GameObject ShotGunModel;
    public GameObject SideArmModel;
    [HideInInspector]
    public Transform ARShootPoint;
    [HideInInspector]
    public Transform MGShootPoint;
    [HideInInspector]
    public Transform FTShootPoint;
    [HideInInspector]
    public Transform[] SGShootPoint;
    public Transform SAShootPoint;
    [HideInInspector]
    public Transform ARCasePoint;
    [HideInInspector]
    public Transform MGCasePoint;
    [HideInInspector]
    public Transform SGCasePoint;
    public Transform SACasePoint;
    public float AssaultRifleFiringTime;
    public float MiniGunFiringTime;
    public float FlameThrowerFiringTime;
    public float ShotGunFiringTime;
    public float SideArmFiringTime;
    public AudioSource WeaponAudioSource;
    public AudioSource PlayerAudioSource;
    [HideInInspector]
    public bool Firing;
    [HideInInspector]
    public bool CanShoot;
    [HideInInspector]
    public ParticleSystem ARBulletParticle;
    [HideInInspector]
    public ParticleSystem ARMuzzleFlash;
    [HideInInspector]
    public ParticleSystem ARBulletCasing;
    [HideInInspector]
    public ParticleSystem MGBulletParticle;
    [HideInInspector]
    public ParticleSystem MGMuzzleFlash;
    [HideInInspector]
    public ParticleSystem MGBulletCasing;
    [HideInInspector]
    public ParticleSystem FTFlame;
    [HideInInspector]
    public ParticleSystem FTHeatDistortion;
    [HideInInspector]
    public ParticleSystem[] SGBulletParticle;
    [HideInInspector]
    public ParticleSystem SGMuzzleFlash;
    [HideInInspector]
    public ParticleSystem SGBulletCasing;
    public ParticleSystem SABulletParticle;
    public ParticleSystem SAMuzzleFlash;
    public ParticleSystem SABulletCasing;
    [HideInInspector]
    public int CurrentDamage;
    [Space]
    public int ARDamage;
    public int MGDamage;
    public int FTDamage;
    public int SGDamage;
    public int SADamage;
    bool Scrolling;
    float ScrollingValue;
    [HideInInspector]
    public Ray RayCastRay;
    [HideInInspector]
    public RaycastHit RayCastHit;
    [HideInInspector]
    public GameObject HitTarget;
    [HideInInspector]
    public GameObject HitEnemy;
    [HideInInspector]
    public float PlayerRotation;
    [HideInInspector]
    public bool FacingUp, FacingDown, FacingLeft, FacingRight,PerkingUp;
    public bool Moving;
    public int CurrentFootStepValue;
    public bool Down;
    [HideInInspector]
    public Transform HeightOcclusionPoint;
    [Space]
    [Header("Server")]
    public _PlayerManager ThisClientManager;


    [Space]
    [Header("Camera Effects")]
    //For camera punch effect
    [Range(0, 1)]
    public float CameraShootEffectValue;
    float TempPunchValue;
    #region SinglePlayer

    void Update()
    {
        if (!SM.GameScript.Server)
        {
            if (SM.GameScript.InGame == true)
            {
                if (SM.GameScript.Paused == false)
                {
                    //Is called when game is unpased and game is currently active
                    if (!Down)
                    {
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
                        if (Moving)
                        {
                            FootStepping();
                        }
                    }
                    if (Health <= 0)
                    {
                        if (Down == false)
                        {
                            Debug.Log("DownMethod");
                            Down = true;
                            SM.EnemyScript.IgnorePlayer = true;
                            PlayerAnimator.SetBool("CanRevive", false);
                            PlayerAnimator.Play("Death", 0);
                            if (SM.CoinScript.CoinAmmount > 0)
                            {
                                Debug.Log("Down");
                                StartCoroutine(PlayerDown());
                            }
                            else
                            {
                                Debug.Log("Dead");
                                PlayerDead();
                            }
                        }
                    }
                }
                else
                {
                    PlayerAnimator.SetBool("Walk", false);
                    PlayerAnimator.SetBool("Firing", false);
                }
            }
        }
    }
    public void PlayerDead() 
    {
        SM.DeathScreenManager.PlayerDead();
    }
   
    IEnumerator PlayerDown()
    {
        SM.GameMenuScript.InsertCoin.SetActive(true);
        while (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.F)) 
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        SM.GameMenuScript.InsertCoin.SetActive(false);
        Health = StartingHealth;
        SM.GameMenuScript.HealthMethod(Health);
        Down = false;
        PlayerAnimator.SetBool("CanRevive", true);
        SM.CoinScript.CoinAmmount = SM.CoinScript.CoinAmmount - 1;
        SM.GameMenuScript.CoinMethod();
        yield return new WaitForSecondsRealtime(3f);
        SM.EnemyScript.IgnorePlayer = false;

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
                ShotGunModel.SetActive(false);
                SideArmModel.SetActive(false);
                CurrentDamage = ARDamage;
                PlayerAnimator.SetBool("Shotgun", false);
                PlayerAnimator.SetBool("SideArm", false);
                break;
            case 2:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(true);
                FlameThrowerModel.SetActive(false);
                ShotGunModel.SetActive(false);
                SideArmModel.SetActive(false);
                CurrentDamage = MGDamage;
                PlayerAnimator.SetBool("Shotgun", false);
                PlayerAnimator.SetBool("SideArm", false);
                break;
            case 3:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(true);
                ShotGunModel.SetActive(false);
                SideArmModel.SetActive(false);
                CurrentDamage = FTDamage;
                PlayerAnimator.SetBool("SideArm", false);
                PlayerAnimator.SetBool("Shotgun", false);
                break;
            case 4:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(false);
                ShotGunModel.SetActive(true);
                SideArmModel.SetActive(false);
                CurrentDamage = SGDamage;
                PlayerAnimator.SetBool("SideArm", false);
                PlayerAnimator.SetBool("Shotgun", true);
                break;
            case 5:
                AutomaticRifleModel.SetActive(false);
                MiniGunModel.SetActive(false);
                FlameThrowerModel.SetActive(false);
                ShotGunModel.SetActive(false);
                SideArmModel.SetActive(true);
                CurrentDamage = SADamage;
                PlayerAnimator.SetBool("Shotgun", false);
                PlayerAnimator.SetBool("SideArm", true);
                break;
        }
    }
    public void FootStepping() 
    {
        Vector3 Down = Player.transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(Player.transform.position, Down, out RayCastHit))
        {
            switch (RayCastHit.transform.gameObject.layer) 
            {
                case 15:
                    CurrentFootStepValue = 0;
                    break;
                case 16:
                    CurrentFootStepValue = 1;
                    break;
                case 17:
                    CurrentFootStepValue = 2;
                    break;
                case 18:
                    CurrentFootStepValue = 3;
                    break;
            }
        }
    }
    public void RayCastMethod() 
    {
        //swtich statment depending on the weapon value to make sure the raycast is shot from the correct point
        //if the raycast hits and enemy it calls the healthmanger method on the enemy script and sets enemy hitted to the raycast collison object in enemy script
        switch (WeaponValue)
        {
            case 1:
                //Rifle RayCast
                if (Physics.Raycast(ARShootPoint.position, ARShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        HitEnemy = RayCastHit.transform.gameObject;
                        SM.EnemyScript.EnemyHited = HitEnemy;
                        if (SM.EnemyScript.HealthCalculation == false)
                        {
                            SM.EnemyScript.TakingDamage = true;
                        }
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
            //MiniGun Raycast
            case 2:
                if (Physics.Raycast(MGShootPoint.position, MGShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        HitEnemy = RayCastHit.transform.gameObject;
                        SM.EnemyScript.EnemyHited = HitEnemy;
                        if (SM.EnemyScript.HealthCalculation == false)
                        {
                            SM.EnemyScript.TakingDamage = true;
                        }
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
            //FlameThrower raycast
            case 3:
                if (Physics.Raycast(FTShootPoint.position, FTShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        HitEnemy = RayCastHit.transform.gameObject;
                        SM.EnemyScript.EnemyHited = HitEnemy;
                        if (SM.EnemyScript.HealthCalculation == false)
                        {
                            SM.EnemyScript.TakingDamage = true;
                        }
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
                //Shotgun Raycast
            case 4:
                for (int i = 0; i < SGShootPoint.Length; i++)
                {
                    if (Physics.Raycast(SGShootPoint[i].position, SGShootPoint[i].transform.forward, out RayCastHit))
                    {
                        if (RayCastHit.transform.tag == "Enemy")
                        {
                            HitEnemy = RayCastHit.transform.gameObject;
                            SM.EnemyScript.EnemyHited = HitEnemy;
                            if (SM.EnemyScript.HealthCalculation == false)
                            {
                                SM.EnemyScript.TakingDamage = true;
                            }
                        }
                        else
                        {
                            HitEnemy = null;
                        }
                    }
                }
                break;
            case 5:
                if (Physics.Raycast(SAShootPoint.position, SAShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        HitEnemy = RayCastHit.transform.gameObject;
                        SM.EnemyScript.EnemyHited = HitEnemy;
                        if (SM.EnemyScript.HealthCalculation == false)
                        {
                            SM.EnemyScript.TakingDamage = true;
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
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Scrolling = true;
            ScrollingValue = Input.GetAxis("Mouse ScrollWheel");
        }
        if (Scrolling) 
        {
            ScrollWeaponSwitchCalculation();
        }
        if (SM.PerksScript.CanPurchase)
        {
            if (!SM.PerksScript.PerkMachineChange)
            {
                if (!SM.PerksScript.PerkBaught[SM.PerksScript.PerkValue])
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
                    {
                        if (SM.PointsScript.Points >= SM.PerksScript.CurrentCostOfPerk)
                        {
                            if (!PerkingUp)
                            {
                                PerkingUp = true;
                                StartCoroutine(DrinkingPerk());
                            }
                        }
                    }
                }
            }
        }
    }
    public void ScrollWeaponSwitchCalculation()
    {
        switch (WeaponValue)
        {
            case 1:
                if (ScrollingValue > 0)
                {
                    WeaponValue = 4;
                }
                else if (ScrollingValue < 0)
                {
                    WeaponValue = 5;
                }
                break;
            case 4:
                if (ScrollingValue > 0)
                {
                    WeaponValue = 5;
                }
                else if (ScrollingValue < 0)
                {
                    WeaponValue = 1;
                }
                break;
            case 5:
                if (ScrollingValue > 0)
                {
                    WeaponValue = 1;
                }
                else if (ScrollingValue < 0)
                {
                    WeaponValue = 4;
                }
                break;
        }
        ScrollingValue = 0;
        Scrolling = false;
        WeaponSwitch();
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
            Moving = true;
        }
        else
        {
            PlayerAnimator.SetBool("Walk", false);
            Moving = false;
        }
        if (Firing == true)
        {
            PlayerAnimator.SetBool("Firing", true);
        }
        else
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
        if (Player != null)
        {
            PlayerRotation = Player.transform.eulerAngles.y;
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
                    SM.AudioScripts.WeaponAudio();
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(AssaultRifleFiringTime);
                    SM.CameraScript.yValue = TempPunchValue;
                    CanShoot = false;
                    break;
                case 2:
                    MGBulletParticle.Emit(1);
                    MGBulletCasing.Emit(1);
                    MGMuzzleFlash.Emit(1);
                    SM.AudioScripts.WeaponAudio();
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(MiniGunFiringTime);
                    SM.CameraScript.yValue = TempPunchValue;
                    CanShoot = false;
                    break;
                case 3:
                    FTFlame.Emit(10);
                    FTHeatDistortion.Emit(1);
                    SM.AudioScripts.WeaponAudio();
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(FlameThrowerFiringTime);
                    SM.CameraScript.yValue = TempPunchValue;
                    CanShoot = false;
                    break;
                case 4:
                    SGBulletParticle[0].Emit(1);
                    SGBulletParticle[1].Emit(1);
                    SGBulletParticle[2].Emit(1);
                    SGBulletParticle[3].Emit(1);
                    SGBulletParticle[4].Emit(1);
                    SGBulletCasing.Emit(1);
                    SGMuzzleFlash.Emit(1);
                    SM.AudioScripts.WeaponAudio();
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(ShotGunFiringTime);
                    SM.CameraScript.yValue = TempPunchValue;
                    CanShoot = false;
                    break;
                case 5:
                    SABulletParticle.Emit(1);
                    SABulletCasing.Emit(1);
                    SAMuzzleFlash.Emit(1);
                    SM.AudioScripts.WeaponAudio();
                    CameraPunch();
                    yield return new WaitForSecondsRealtime(SideArmFiringTime);
                    SM.CameraScript.yValue = TempPunchValue;
                    CanShoot = false;
                    break;
            }
            Firing = false;
        }

    }
    IEnumerator DrinkingPerk() 
    {
        SM.PerksScript.PerkBaught[SM.PerksScript.PerkValue] = true;
        SM.PointsScript.Points = SM.PointsScript.Points - SM.PerksScript.CurrentCostOfPerk;
        SM.GameMenuScript.ScoreText.text =SM.PointsScript.Points.ToString();
        SM.PerksScript.PlayerPerkBaught();
        PlayerAnimator.SetBool("Perk", true);
        yield return new WaitForSecondsRealtime(0.1f);
        PlayerAnimator.SetBool("Perk", false);
        yield return new WaitForSecondsRealtime(3f);
        PerkingUp = false;
    }
    public void CameraDamage()
    {
        //method that changes the camera zoom to indicate the player has taken damage
        SM.CameraScript.yValue -= CameraDamageEffectValue;
    }
    public void CameraPunch() 
    {
        //method that changes the zoom level of the camera to show that the player is firing
        TempPunchValue = SM.CameraScript.yValue;
        SM.CameraScript.yValue -= CameraShootEffectValue;
    }
    #endregion
    #region Multiplayer
    public void FixedUpdate()
    {
        if (SM.GameScript.Server)
        {
            if (SM.GameScript.PlayerInstantiated)
            {
                SendKeyInputToServer();
                PlayerRotationalCalculation();
                SendRotationInputToServer();
                DetectScrollWheel();
                DetectMouseButton();
            }
        }
    }
    private void DetectScrollWheel() 
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Scrolling = true;
            ScrollingValue = Input.GetAxis("Mouse ScrollWheel");
        }
        if (Scrolling)
        {
            ScrollWeaponSwitch();
        }
    }
    private void ScrollWeaponSwitch() 
    {
        int NewWeaponValue;
        switch (ThisClientManager.WeaponValue)
        {
            case 1:
                if (ScrollingValue > 0)
                {
                    NewWeaponValue = 4;
                    ClientSend.WeaponSwitch(NewWeaponValue);
                }
                else if (ScrollingValue < 0)
                {
                    NewWeaponValue = 5;
                    ClientSend.WeaponSwitch(NewWeaponValue);
                }
                break;
            case 4:
                if (ScrollingValue > 0)
                {
                    NewWeaponValue = 5;
                    ClientSend.WeaponSwitch(NewWeaponValue);
                }
                else if (ScrollingValue < 0)
                {
                    NewWeaponValue = 1;
                    ClientSend.WeaponSwitch(NewWeaponValue);
                }
                break;
            case 5:
                if (ScrollingValue > 0)
                {
                    NewWeaponValue = 1;
                    ClientSend.WeaponSwitch(NewWeaponValue);
                }
                else if (ScrollingValue < 0)
                {
                    NewWeaponValue = 4;
                    ClientSend.WeaponSwitch(NewWeaponValue);
                }
                break;
        }
        Debug.Log("ISSCROLLING");
        ScrollingValue = 0;
        Scrolling = false;
    }
    private void SendKeyInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.F)
        };
        ClientSend.PlayerMovement(_inputs);
    }
    private void SendRotationInputToServer() 
    {
        Debug.Log("Method Being Called");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            Debug.Log("SendRotation");
            Vector3 MousePos = new Vector3 (hit.point.x, Player.transform.position.y, hit.point.z);
            ClientSend.PlayerRotation(MousePos);
        }
    }
    private void DetectMouseButton()
    {
        if (Input.GetMouseButton(0))
        {
            bool MouseDown = true; 
            ClientSend.MouseIsDown(MouseDown);
        }
        else 
        {
            bool MouseDown = false;
            ClientSend.MouseIsDown(MouseDown);
        }
    }
    #endregion
}
