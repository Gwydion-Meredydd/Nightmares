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
    [Space]
    public ScriptsManager SM;
    [Header("Player Controllers")]
    [Range(1, 4)]
    public int PlayerTypeValue;
    public GameObject[] PlayerTypes;
    public Transform PlayerStartSpawn;
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
    public GameObject MonsterPrefab;
    public GameObject[] PerkMachinePrefabs;
    public GameObject[] EnemieDrops;

    void Update()
    {
        if (AutoStart == true) //for testing purposes instead of setting the start bool to true in the inspector this can be left on to start the game on play
        {
            AutoStart = false;
            StartGame = true;
        }
        if (StartGame == true) 
        {
            //spawns the correct player model depneding on the selected player in the main menu in the starting position
            Vector3 PlayerStartingPosVector3 = PlayerStartSpawn.position;
            Instantiate(PlayerTypes[PlayerTypeValue - 1], PlayerStartingPosVector3, PlayerStartSpawn.rotation);
            SM.PlayerScript.Player = GameObject.FindGameObjectWithTag("Player");
            SM.PlayerScript.PlayerCc = SM.PlayerScript.Player.GetComponent<CharacterController>();

            //find the selected players model's weapons
            SM.PlayerScript.AutomaticRifleModel = GameObject.FindGameObjectWithTag("AR");
            SM.PlayerScript.MiniGunModel = GameObject.FindGameObjectWithTag("MG");
            SM.PlayerScript.FlameThrowerModel = GameObject.FindGameObjectWithTag("FT");

            #region Assault Rifle Starting Initlisation
            //gets all the correct variables for the player controller by using prefabs and tags to put the correct things in the correct place
            TempShootPoint = GameObject.FindGameObjectWithTag("ARShootPoint");
            SM.PlayerScript.ARShootPoint = TempShootPoint.transform;
            ChildTempForShootPoint = Instantiate(ARBulletParticle.gameObject, SM.PlayerScript.ARShootPoint.position, SM.PlayerScript.ARShootPoint.rotation);
            ChildTempForShootPoint.transform.parent = SM.PlayerScript.ARShootPoint;
            SM.PlayerScript.ARBulletParticle = ChildTempForShootPoint.GetComponent<ParticleSystem>();
            TempShootPoint = null;
            ChildTempForShootPoint = null;

            SM.PlayerScript.AssaultRifleAudioSource = SM.PlayerScript.AutomaticRifleModel.GetComponent<AudioSource>();
            SM.PlayerScript.MininGunAudioSource = SM.PlayerScript.MiniGunModel.GetComponent<AudioSource>();
            SM.PlayerScript.FlameThrowerAudioSource = SM.PlayerScript.FlameThrowerModel.GetComponent<AudioSource>();

            ChildTempForMuzzleFlash = Instantiate(ArMuzzleFlash.gameObject, SM.PlayerScript.ARShootPoint.position, SM.PlayerScript.ARShootPoint.rotation);
            ChildTempForMuzzleFlash.transform.parent = SM.PlayerScript.ARShootPoint;
            SM.PlayerScript.ARMuzzleFlash = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
            ChildTempForMuzzleFlash = null;

            TempShellPoint = GameObject.FindGameObjectWithTag("ARCasePoint");
            SM.PlayerScript.ARCasePoint = TempShellPoint.transform;
            ChildTempForCasePoint = Instantiate(ARShellParticle.gameObject, SM.PlayerScript.ARCasePoint.position, SM.PlayerScript.ARCasePoint.rotation);
            ChildTempForCasePoint.transform.parent = SM.PlayerScript.ARCasePoint;
            SM.PlayerScript.ARBulletCasing = ChildTempForCasePoint.GetComponent<ParticleSystem>();
            TempShellPoint = null;
            ChildTempForCasePoint = null;
            #endregion

            #region Mini Gun Starting Initlisation
            //gets all the correct variables for the player controller by using prefabs and tags to put the correct things in the correct place
            TempShootPoint = GameObject.FindGameObjectWithTag("MGShootPoint");
            SM.PlayerScript.MGShootPoint = TempShootPoint.transform;
            ChildTempForShootPoint = Instantiate(MGBulletParticle.gameObject, SM.PlayerScript.MGShootPoint.position, SM.PlayerScript.MGShootPoint.rotation);
            ChildTempForShootPoint.transform.parent = SM.PlayerScript.MGShootPoint;
            SM.PlayerScript.MGBulletParticle = ChildTempForShootPoint.GetComponent<ParticleSystem>();
            TempShootPoint = null;
            ChildTempForShootPoint = null;


            ChildTempForMuzzleFlash = Instantiate(MGMuzzleFlash.gameObject, SM.PlayerScript.MGShootPoint.position, SM.PlayerScript.MGShootPoint.rotation);
            ChildTempForMuzzleFlash.transform.parent = SM.PlayerScript.MGShootPoint;
            SM.PlayerScript.MGMuzzleFlash = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
            ChildTempForMuzzleFlash = null;

            TempShellPoint = GameObject.FindGameObjectWithTag("MGCasePoint");
            SM.PlayerScript.MGCasePoint = TempShellPoint.transform;
            ChildTempForCasePoint = Instantiate(MGShellParticle.gameObject, SM.PlayerScript.MGCasePoint.position, SM.PlayerScript.MGCasePoint.rotation);
            ChildTempForCasePoint.transform.parent = SM.PlayerScript.MGCasePoint;
            SM.PlayerScript.MGBulletCasing = ChildTempForCasePoint.GetComponent<ParticleSystem>();
            TempShellPoint = null;
            ChildTempForCasePoint = null;
            #endregion

            #region Flame Thrower Starting Initlisation
            //gets all the correct variables for the player controller by using prefabs and tags to put the correct things in the correct place
            TempShootPoint = GameObject.FindGameObjectWithTag("FTShootPoint");
            SM.PlayerScript.FTShootPoint = TempShootPoint.transform;
            ChildTempForShootPoint = Instantiate(FTFlame.gameObject, SM.PlayerScript.FTShootPoint.position, SM.PlayerScript.FTShootPoint.rotation);
            ChildTempForShootPoint.transform.parent = SM.PlayerScript.FTShootPoint;
            SM.PlayerScript.FTFlame = ChildTempForShootPoint.GetComponent<ParticleSystem>();
            TempShootPoint = null;
            ChildTempForShootPoint = null;


            ChildTempForMuzzleFlash = Instantiate(FTHeatDistortion.gameObject, SM.PlayerScript.FTShootPoint.position, SM.PlayerScript.FTShootPoint.rotation);
            ChildTempForMuzzleFlash.transform.parent = SM.PlayerScript.FTShootPoint;
            SM.PlayerScript.FTHeatDistortion = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
            ChildTempForMuzzleFlash = null;

            #endregion

            //sets special settings for other scripts such as camera script
            SM.EnemySpawningScript.MonsterPrefab = MonsterPrefab;
            SM.PerksScript.PerkMachine = new GameObject[4];
            SM.PerksScript.PerkMachineAnimator = new Animator[4];
            SM.PerksScript.PerkBaught = new bool[4];
            GameObject TempPerkGameObject = GameObject.FindGameObjectWithTag("PerkSpawnPoint");
            SM.PerksScript.PerkSpawnPoint = TempPerkGameObject.transform;
            SM.PerksScript.PerkSpawnQuaternion = SM.PerksScript.PerkSpawnPoint.rotation;
            SM.PerksScript.PerkSpawnVector = SM.PerksScript.PerkSpawnPoint.position;
            for (int i = 0; i < 4; i++)
            {
                Instantiate(PerkMachinePrefabs[i], SM.PerksScript.PerkSpawnVector, SM.PerksScript.PerkSpawnQuaternion);
            }
            SM.PerksScript.PerkMachine = GameObject.FindGameObjectsWithTag("PerkMachine");
            for (int i = 0; i < 4; i++)
            {
                SM.PerksScript.PerkMachineAnimator[i] = SM.PerksScript.PerkMachine[i].GetComponent<Animator>();
            }
            for (int i = 0; i < 5; i++) 
            {
                SM.DropsScript.Drops[i] = EnemieDrops[i];
            }
            SM.PerksScript.NewRound();
            SM.CameraScript.player = SM.PlayerScript.Player.transform;
            StartGame = false;
            InGame = true;
            SM.PlayerScript.PlayerAnimator = SM.PlayerScript.Player.GetComponent<Animator>();
            SM.PlayerScript.WeaponHeldValue = SM.PlayerScript.WeaponValue;
            SM.PlayerScript.WeaponSwitch();
            SM.PlayerScript.Health = SM.PlayerScript.StartingHealth;
            SM.CameraScript.HoldingYValue = SM.CameraScript.yValue;
            GameObject[] TempSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            SM.EnemySpawningScript.LevelSpawnPoints = new Transform[TempSpawnPoints.Length];
            for (int i = 0; i < TempSpawnPoints.Length; i++)
            {
                SM.EnemySpawningScript.LevelSpawnPoints[i] = TempSpawnPoints[i].transform;
            }

            switch (SM.PlayerScript.WeaponValue)//checks which gun is active (this is mostly for testing purposes since the player will allways start with the dafult rifle)
            {
                case 1:
                    SM.PlayerScript.AutomaticRifleModel.SetActive(true);
                    SM.PlayerScript.MiniGunModel.SetActive(false);
                    SM.PlayerScript.FlameThrowerModel.SetActive(false);
                    break;
                case 2:
                    SM.PlayerScript.AutomaticRifleModel.SetActive(false);
                    SM.PlayerScript.MiniGunModel.SetActive(true);
                    SM.PlayerScript.FlameThrowerModel.SetActive(false);
                    break;
                case 3:
                    SM.PlayerScript.AutomaticRifleModel.SetActive(false);
                    SM.PlayerScript.MiniGunModel.SetActive(false);
                    SM.PlayerScript.FlameThrowerModel.SetActive(true);
                    break;
            }
        }   
    }
}
