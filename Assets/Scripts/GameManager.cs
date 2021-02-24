using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Controllers")]
    public bool InitliseGame;
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
    public GameObject PlayerStartSpawn;
    public ParticleSystem ARBulletParticle;
    public ParticleSystem ArMuzzleFlash;
    public ParticleSystem ARShellParticle;
    public ParticleSystem MGBulletParticle;
    public ParticleSystem MGMuzzleFlash;
    public ParticleSystem MGShellParticle;
    public ParticleSystem FTFlame;
    public ParticleSystem FTHeatDistortion;
    public ParticleSystem SGBulletParticle;
    public ParticleSystem SGMuzzleFlash;
    public ParticleSystem SGShellParticle;
    private GameObject TempShootPoint;
    private GameObject ChildTempForShootPoint;
    private GameObject ChildTempForMuzzleFlash;
    private GameObject TempShellPoint;
    private GameObject ChildTempForCasePoint;
    public GameObject[] MonsterPrefabs;
    public GameObject[] PerkMachinePrefabs;
    public GameObject[] EnemieDrops;
    public GameObject[] Levels;
    [Space]
    [Header("Menus")]
    public GameObject MainMenu;
    public GameObject GameMenu;
    [Space]
    [Header("Server")]
    public bool Server;
    public bool SGMInstantiated;
    public bool PlayerInstantiated;
    public bool CameraInstantiated;
    public bool LevelInstanitated;
    [Space]
    public static GameManager instance;

    public static Dictionary<int, _PlayerManager> players = new Dictionary<int, _PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    void Update()
    {
        if (!Server)
        {
            if (!InitliseGame)
            {
                MainMenu.SetActive(true);
                GameMenu.SetActive(false);
                SM.AudioScripts.MainMenuMusic.SetActive(true);
                InitliseGame = true;
            }
            if (AutoStart == true) //for testing purposes instead of setting the start bool to true in the inspector this can be left on to start the game on play
            {
                AutoStart = false;
                StartGame = true;
            }
            if (StartGame == true)
            {
                SM.AudioScripts.MainMenuMusic.SetActive(false);
                MainMenu.SetActive(false);
                GameMenu.SetActive(true);
                SM.LevelScript.Levels = new GameObject[Levels.Length];
                for (int i = 0; i < Levels.Length; i++)
                {
                    if (Levels[i] != null)
                    {
                        SM.LevelScript.Levels[i] = Levels[i];
                    }
                }
                SM.LevelScript.SpawnLevel();
                SM.LevelScript.HeightOcclusionObjects = GameObject.FindGameObjectsWithTag("HeightOcclusion");
                SM.LevelScript.HeightOcclusionReplacementObjects = GameObject.FindGameObjectsWithTag("HeightOcclusionReplacement");
                foreach (var HeightOcclusionReplacementObjects in SM.LevelScript.HeightOcclusionReplacementObjects)
                {
                    HeightOcclusionReplacementObjects.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }

                //spawns the correct player model depneding on the selected player in the main menu in the starting position
                PlayerStartSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
                Vector3 PlayerStartingPosVector3 = PlayerStartSpawn.transform.position;
                Instantiate(PlayerTypes[PlayerTypeValue - 1], PlayerStartingPosVector3, PlayerStartSpawn.transform.rotation);
                SM.PlayerScript.Player = GameObject.FindGameObjectWithTag("Player");
                SM.PlayerScript.PlayerCc = SM.PlayerScript.Player.GetComponent<CharacterController>();

                //find the selected players model's weapons
                SM.PlayerScript.AutomaticRifleModel = GameObject.FindGameObjectWithTag("AR");
                SM.PlayerScript.MiniGunModel = GameObject.FindGameObjectWithTag("MG");
                SM.PlayerScript.FlameThrowerModel = GameObject.FindGameObjectWithTag("FT");
                SM.PlayerScript.ShotGunModel = GameObject.FindGameObjectWithTag("SG");
                SM.PlayerScript.SideArmModel = GameObject.FindGameObjectWithTag("SA");

                #region Assault Rifle Starting Initlisation
                //gets all the correct variables for the player controller by using prefabs and tags to put the correct things in the correct place
                TempShootPoint = GameObject.FindGameObjectWithTag("ARShootPoint");
                SM.PlayerScript.ARShootPoint = TempShootPoint.transform;
                ChildTempForShootPoint = Instantiate(ARBulletParticle.gameObject, SM.PlayerScript.ARShootPoint.position, SM.PlayerScript.ARShootPoint.rotation);
                ChildTempForShootPoint.transform.parent = SM.PlayerScript.ARShootPoint;
                SM.PlayerScript.ARBulletParticle = ChildTempForShootPoint.GetComponent<ParticleSystem>();
                TempShootPoint = null;
                ChildTempForShootPoint = null;

                GameObject TempAudioGameObject = GameObject.FindGameObjectWithTag("WeaponMaster");
                SM.PlayerScript.WeaponAudioSource = TempAudioGameObject.GetComponent<AudioSource>();
                SM.PlayerScript.PlayerAudioSource = SM.PlayerScript.Player.GetComponent<AudioSource>();

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

                #region ShotGun Starting Initlisation
                //gets all the correct variables for the player controller by using prefabs and tags to put the correct things in the correct place
                GameObject[] ShotgunTempShootPoint = GameObject.FindGameObjectsWithTag("SGShootPoint");
                SM.PlayerScript.SGShootPoint = new Transform[ShotgunTempShootPoint.Length];
                for (int i = 0; i < ShotgunTempShootPoint.Length; i++)
                {
                    SM.PlayerScript.SGShootPoint[i] = ShotgunTempShootPoint[i].transform;
                    ChildTempForShootPoint = Instantiate(SGBulletParticle.gameObject, SM.PlayerScript.SGShootPoint[i].position, SM.PlayerScript.SGShootPoint[i].rotation);
                    ChildTempForShootPoint.transform.parent = SM.PlayerScript.SGShootPoint[i];
                    SM.PlayerScript.SGBulletParticle[i] = ChildTempForShootPoint.GetComponent<ParticleSystem>();
                    ChildTempForShootPoint = null;
                }
                TempShootPoint = null;

                ChildTempForMuzzleFlash = Instantiate(ArMuzzleFlash.gameObject, SM.PlayerScript.SGShootPoint[0].position, SM.PlayerScript.SGShootPoint[0].rotation);
                ChildTempForMuzzleFlash.transform.parent = SM.PlayerScript.SGShootPoint[0];
                SM.PlayerScript.SGMuzzleFlash = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
                ChildTempForMuzzleFlash = null;

                TempShellPoint = GameObject.FindGameObjectWithTag("SGCasePoint");
                SM.PlayerScript.SGCasePoint = TempShellPoint.transform;
                ChildTempForCasePoint = Instantiate(ARShellParticle.gameObject, SM.PlayerScript.SGCasePoint.position, SM.PlayerScript.SGCasePoint.rotation);
                ChildTempForCasePoint.transform.parent = SM.PlayerScript.SGCasePoint;
                SM.PlayerScript.SGBulletCasing = ChildTempForCasePoint.GetComponent<ParticleSystem>();
                TempShellPoint = null;
                ChildTempForCasePoint = null;
                #endregion

                #region Side Arm Starting Initlisation
                //gets all the correct variables for the player controller by using prefabs and tags to put the correct things in the correct place
                TempShootPoint = GameObject.FindGameObjectWithTag("SAShootPoint");
                SM.PlayerScript.SAShootPoint = TempShootPoint.transform;
                ChildTempForShootPoint = Instantiate(ARBulletParticle.gameObject, SM.PlayerScript.SAShootPoint.position, SM.PlayerScript.SAShootPoint.rotation);
                ChildTempForShootPoint.transform.parent = SM.PlayerScript.SAShootPoint;
                SM.PlayerScript.SABulletParticle = ChildTempForShootPoint.GetComponent<ParticleSystem>();
                TempShootPoint = null;
                ChildTempForShootPoint = null;

                ChildTempForMuzzleFlash = Instantiate(ArMuzzleFlash.gameObject, SM.PlayerScript.SAShootPoint.position, SM.PlayerScript.SAShootPoint.rotation);
                ChildTempForMuzzleFlash.transform.parent = SM.PlayerScript.SAShootPoint;
                SM.PlayerScript.SAMuzzleFlash = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
                ChildTempForMuzzleFlash = null;

                TempShellPoint = GameObject.FindGameObjectWithTag("SACasePoint");
                SM.PlayerScript.SACasePoint = TempShellPoint.transform;
                ChildTempForCasePoint = Instantiate(ARShellParticle.gameObject, SM.PlayerScript.SACasePoint.position, SM.PlayerScript.SACasePoint.rotation);
                ChildTempForCasePoint.transform.parent = SM.PlayerScript.SACasePoint;
                SM.PlayerScript.SABulletCasing = ChildTempForCasePoint.GetComponent<ParticleSystem>();
                TempShellPoint = null;
                ChildTempForCasePoint = null;
                #endregion
                //sets special settings for other scripts such as camera script
                SM.EnemySpawningScript.MonsterPrefab = new GameObject[3];
                SM.EnemySpawningScript.MonsterPrefab[0] = MonsterPrefabs[0];
                SM.EnemySpawningScript.MonsterPrefab[2] = MonsterPrefabs[1];
                SM.EnemySpawningScript.MonsterPrefab[1] = MonsterPrefabs[2];
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
                for (int i = 0; i < 6; i++)
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
                SM.GameMenuScript.HealthMethod(SM.PlayerScript.Health);
                GameObject HeightOcclusionPointTemp = GameObject.FindGameObjectWithTag("HeightOcclusionPoint");
                SM.PlayerScript.HeightOcclusionPoint = HeightOcclusionPointTemp.transform;
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
                StartGame = false;
            }
        }
        else if (Server)
        {
            if (!SGMInstantiated) 
            {
                SGMInstantiated = true;
                if (instance == null)
                {
                    instance = this;
                }
                else if (instance != this)
                {
                    Debug.Log("Instance already exists, destryoing Object!");
                    Destroy(this);
                }
            }
            if (PlayerInstantiated) 
            {
                GameObject TempPlayer = GameObject.FindGameObjectWithTag("Player");
                SM.CameraScript.player = TempPlayer.transform;
                CameraInstantiated = true;
            }
            if (!LevelInstanitated) 
            {
                LevelInstanitated = true;
                SM.LevelScript.SpawnLevel();
            }
        }
    }
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
       // SM.HostingManager.SwitchScene();
        GameObject _player;
        if (_id == ClientManager.instance.myID)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }
        _player.GetComponent<_PlayerManager>().id = _id;
        _player.GetComponent<_PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<_PlayerManager>());
        SM.PlayerScript.Player = _player;
        PlayerInstantiated = true;
       // SM.HostingManager.SwitchScene();
    }
}
