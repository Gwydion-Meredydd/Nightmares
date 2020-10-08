using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("Game Controllers")]
    public bool Paused;
    public bool AutoStart;
    public bool StartGame;
    public bool InGame;

    [Header("Player Controllers")]
    [Range(1, 4)]
    public int PlayerTypeValue;
    public GameObject[] PlayerTypes;
    public Transform PlayerStartSpawn;
    public PlayerController PlayerController;
    public CameraController camera_Controller;
    public EnemyScript enemyScript;
    public ParticleSystem ARBulletParticle;
    public ParticleSystem ArMuzzleFlash;
    public ParticleSystem ARShellParticle;
    public ParticleSystem MGBulletParticle;
    public ParticleSystem MGMuzzleFlash;
    public ParticleSystem MGShellParticle;
    public ParticleSystem FTFlame;
    public ParticleSystem FTHeatDistortion;
    private GameObject TempShootPoint;
    private GameObject ChildTempForShootPoint;
    private GameObject ChildTempForMuzzleFlash;
    private GameObject TempShellPoint;
    private GameObject ChildTempForCasePoint;

    void Update()
    {
        if (AutoStart == true) 
        {
            AutoStart = false;
            StartGame = true;
        }
        if (StartGame == true) 
        {
            Vector3 PlayerStartingPosVector3 = PlayerStartSpawn.position;
            Instantiate(PlayerTypes[PlayerTypeValue - 1], PlayerStartingPosVector3, PlayerStartSpawn.rotation);
            PlayerController.Player = GameObject.FindGameObjectWithTag("Player");
            PlayerController.PlayerCc = PlayerController.Player.GetComponent<CharacterController>();

            PlayerController.AutomaticRifleModel = GameObject.FindGameObjectWithTag("AR");
            PlayerController.MiniGunModel = GameObject.FindGameObjectWithTag("MG");
            PlayerController.FlameThrowerModel = GameObject.FindGameObjectWithTag("FT");

            #region Assault Rifle Starting Initlisation

            TempShootPoint = GameObject.FindGameObjectWithTag("ARShootPoint");
            PlayerController.ARShootPoint = TempShootPoint.transform;
            ChildTempForShootPoint = Instantiate(ARBulletParticle.gameObject, PlayerController.ARShootPoint.position, PlayerController.ARShootPoint.rotation);
            ChildTempForShootPoint.transform.parent = PlayerController.ARShootPoint;
            PlayerController.ARBulletParticle = ChildTempForShootPoint.GetComponent<ParticleSystem>();
            TempShootPoint = null;
            ChildTempForShootPoint = null;


            ChildTempForMuzzleFlash = Instantiate(ArMuzzleFlash.gameObject, PlayerController.ARShootPoint.position, PlayerController.ARShootPoint.rotation);
            ChildTempForMuzzleFlash.transform.parent = PlayerController.ARShootPoint;
            PlayerController.ARMuzzleFlash = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
            ChildTempForMuzzleFlash = null;

            TempShellPoint = GameObject.FindGameObjectWithTag("ARCasePoint");
            PlayerController.ARCasePoint = TempShellPoint.transform;
            ChildTempForCasePoint = Instantiate(ARShellParticle.gameObject, PlayerController.ARCasePoint.position, PlayerController.ARCasePoint.rotation);
            ChildTempForCasePoint.transform.parent = PlayerController.ARCasePoint;
            PlayerController.ARBulletCasing = ChildTempForCasePoint.GetComponent<ParticleSystem>();
            TempShellPoint = null;
            ChildTempForCasePoint = null;
            #endregion

            #region Mini Gun Starting Initlisation

            TempShootPoint = GameObject.FindGameObjectWithTag("MGShootPoint");
            PlayerController.MGShootPoint = TempShootPoint.transform;
            ChildTempForShootPoint = Instantiate(MGBulletParticle.gameObject, PlayerController.MGShootPoint.position, PlayerController.MGShootPoint.rotation);
            ChildTempForShootPoint.transform.parent = PlayerController.MGShootPoint;
            PlayerController.MGBulletParticle = ChildTempForShootPoint.GetComponent<ParticleSystem>();
            TempShootPoint = null;
            ChildTempForShootPoint = null;


            ChildTempForMuzzleFlash = Instantiate(MGMuzzleFlash.gameObject, PlayerController.MGShootPoint.position, PlayerController.MGShootPoint.rotation);
            ChildTempForMuzzleFlash.transform.parent = PlayerController.MGShootPoint;
            PlayerController.MGMuzzleFlash = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
            ChildTempForMuzzleFlash = null;

            TempShellPoint = GameObject.FindGameObjectWithTag("MGCasePoint");
            PlayerController.MGCasePoint = TempShellPoint.transform;
            ChildTempForCasePoint = Instantiate(MGShellParticle.gameObject, PlayerController.MGCasePoint.position, PlayerController.MGCasePoint.rotation);
            ChildTempForCasePoint.transform.parent = PlayerController.MGCasePoint;
            PlayerController.MGBulletCasing = ChildTempForCasePoint.GetComponent<ParticleSystem>();
            TempShellPoint = null;
            ChildTempForCasePoint = null;
            #endregion

            #region Flame Thrower Starting Initlisation

            TempShootPoint = GameObject.FindGameObjectWithTag("FTShootPoint");
            PlayerController.FTShootPoint = TempShootPoint.transform;
            ChildTempForShootPoint = Instantiate(FTFlame.gameObject, PlayerController.FTShootPoint.position, PlayerController.FTShootPoint.rotation);
            ChildTempForShootPoint.transform.parent = PlayerController.FTShootPoint;
            PlayerController.FTFlame = ChildTempForShootPoint.GetComponent<ParticleSystem>();
            TempShootPoint = null;
            ChildTempForShootPoint = null;


            ChildTempForMuzzleFlash = Instantiate(FTHeatDistortion.gameObject, PlayerController.FTShootPoint.position, PlayerController.FTShootPoint.rotation);
            ChildTempForMuzzleFlash.transform.parent = PlayerController.FTShootPoint;
            PlayerController.FTHeatDistortion = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
            ChildTempForMuzzleFlash = null;

            #endregion

            camera_Controller.player = PlayerController.Player.transform;
            enemyScript.player = PlayerController.Player.transform;
            enemyScript.canSpawn = true;
            StartGame = false;
            InGame = true;
            PlayerController.WeaponHeldValue = PlayerController.WeaponValue;
            switch (PlayerController.WeaponValue)
            {
                case 1:
                    PlayerController.AutomaticRifleModel.SetActive(true);
                    PlayerController.MiniGunModel.SetActive(false);
                    PlayerController.FlameThrowerModel.SetActive(false);
                    break;
                case 2:
                    PlayerController.AutomaticRifleModel.SetActive(false);
                    PlayerController.MiniGunModel.SetActive(true);
                    PlayerController.FlameThrowerModel.SetActive(false);
                    break;
                case 3:
                    PlayerController.AutomaticRifleModel.SetActive(false);
                    PlayerController.MiniGunModel.SetActive(false);
                    PlayerController.FlameThrowerModel.SetActive(true);
                    break;
            }
        }   
    }
}
