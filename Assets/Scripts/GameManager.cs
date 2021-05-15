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
    public GameObject[] GroundMonsterPrefabs;
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

    public GameObject[] ClientPlayers;

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
                SM.EnemySpawningScript.GroundMonsterPrefab = new GameObject[3];
                SM.EnemySpawningScript.MonsterPrefab[0] = MonsterPrefabs[0];
                SM.EnemySpawningScript.MonsterPrefab[2] = MonsterPrefabs[1];
                SM.EnemySpawningScript.MonsterPrefab[1] = MonsterPrefabs[2];
                SM.EnemySpawningScript.GroundMonsterPrefab[0] = GroundMonsterPrefabs[0];
                SM.EnemySpawningScript.GroundMonsterPrefab[2] = GroundMonsterPrefabs[1];
                SM.EnemySpawningScript.GroundMonsterPrefab[1] = GroundMonsterPrefabs[2];
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
            if (!LevelInstanitated) 
            {
                LevelInstanitated = true;            }
        }
    }
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == ClientManager.instance.myID)
        {
            switch (_id)
            {
                case 1:
                    _player = Instantiate(ClientPlayers[0], _position, _rotation);
                    SM.CameraScript.player = _player.transform;
                    _player.GetComponent<_PlayerManager>().id = _id;
                    _player.GetComponent<_PlayerManager>().username = _username;
                    players.Add(_id, _player.GetComponent<_PlayerManager>());
                    players[_id].IsClient = true;
                    SM.PlayerScript.Player = _player;
                    SM.PlayerScript.ThisClientManager = _player.GetComponent<_PlayerManager>();
                    MultiplayerClientsInstatiate(_id, _player);
                    break;
                case 2:
                    _player = Instantiate(ClientPlayers[1], _position, _rotation);
                    SM.CameraScript.player = _player.transform;
                    _player.GetComponent<_PlayerManager>().id = _id;
                    _player.GetComponent<_PlayerManager>().username = _username;
                    players.Add(_id, _player.GetComponent<_PlayerManager>());
                    players[_id].IsClient = true;
                    SM.PlayerScript.Player = _player;
                    SM.PlayerScript.ThisClientManager = _player.GetComponent<_PlayerManager>();
                    MultiplayerClientsInstatiate(_id, _player);
                    break;
                case 3:
                    _player = Instantiate(ClientPlayers[2], _position, _rotation);
                    SM.CameraScript.player = _player.transform;
                    _player.GetComponent<_PlayerManager>().id = _id;
                    _player.GetComponent<_PlayerManager>().username = _username;
                    players.Add(_id, _player.GetComponent<_PlayerManager>());
                    players[_id].IsClient = true;
                    SM.PlayerScript.Player = _player;
                    SM.PlayerScript.ThisClientManager = _player.GetComponent<_PlayerManager>();
                    MultiplayerClientsInstatiate(_id, _player);
                    break;
                case 4:
                    _player = Instantiate(ClientPlayers[3], _position, _rotation);
                    SM.CameraScript.player = _player.transform;
                    _player.GetComponent<_PlayerManager>().id = _id;
                    _player.GetComponent<_PlayerManager>().username = _username;
                    players.Add(_id, _player.GetComponent<_PlayerManager>());
                    players[_id].IsClient = true;
                    SM.PlayerScript.Player = _player;
                    SM.PlayerScript.ThisClientManager = _player.GetComponent<_PlayerManager>();
                    MultiplayerClientsInstatiate(_id, _player);
                    break;
            }
            CameraInstantiated = true;
        }
        else
        {
            switch (_id)
            {
                case 1:
                    _player = Instantiate(ClientPlayers[0], _position, _rotation);
                    players.Add(_id, _player.GetComponent<_PlayerManager>());
                    players[_id].IsClient = false;
                    players[_id].id = _id;
                    players[_id].username = SM.multiplayerManager.Username[0].ToString();
                    MultiplayerClientsInstatiate(_id, _player);
                    break;
                case 2:
                    _player = Instantiate(ClientPlayers[1], _position, _rotation);
                    players.Add(_id, _player.GetComponent<_PlayerManager>());
                    players[_id].IsClient = false;
                    players[_id].id = _id;
                    players[_id].username = SM.multiplayerManager.Username[1].ToString();
                    MultiplayerClientsInstatiate(_id, _player);
                    break;
                case 3:
                    _player = Instantiate(ClientPlayers[2], _position, _rotation);
                    players.Add(_id, _player.GetComponent<_PlayerManager>());
                    players[_id].IsClient = false;
                    players[_id].id = _id;
                    players[_id].username = SM.multiplayerManager.Username[2].ToString();
                    MultiplayerClientsInstatiate(_id, _player);
                    break;
                case 4:
                    _player = Instantiate(ClientPlayers[3], _position, _rotation);
                    players.Add(_id, _player.GetComponent<_PlayerManager>());
                    players[_id].IsClient = false;
                    players[_id].id = _id;
                    players[_id].username = SM.multiplayerManager.Username[3].ToString();
                    MultiplayerClientsInstatiate(_id, _player);

                    break;
            }
        }
        PlayerInstantiated = true;
    }
    private void MultiplayerClientsInstatiate(int _id, GameObject _player) 
    {
        players[_id].AutomaticRifleModel = FindGameObject(_player, "AR").gameObject;
        players[_id].MiniGunModel = FindGameObject(_player, "MG").gameObject;
        players[_id].FlameThrowerModel = FindGameObject(_player, "FT").gameObject;
        players[_id].ShotGunModel = FindGameObject(_player, "SG").gameObject;
        players[_id].SideArmModel = FindGameObject(_player, "SA").gameObject;

        GameObject ClientTempAudioGameObject = FindGameObject(players[_id].gameObject, "WeaponMaster").gameObject;
        players[_id].WeaponAudioSource = ClientTempAudioGameObject.GetComponent<AudioSource>();
        players[_id].PlayerAudioSource = players[_id].GetComponent<AudioSource>();

        #region AssaultRifle
        players[_id].ARShootPoint = FindGameObject(players[_id].AutomaticRifleModel, "ARShootPoint").transform;
        GameObject ClientChildTempForShootPoint = Instantiate(ARBulletParticle.gameObject, players[_id].ARShootPoint.position, players[_id].ARShootPoint.rotation);
        ClientChildTempForShootPoint.transform.parent = players[_id].ARShootPoint;
        players[_id].ARBulletParticle = ClientChildTempForShootPoint.GetComponent<ParticleSystem>();
        ClientChildTempForShootPoint = null;

        GameObject ClientChildTempForMuzzleFlash = Instantiate(ArMuzzleFlash.gameObject, players[_id].ARShootPoint.position, players[_id].ARShootPoint.rotation);
        ClientChildTempForMuzzleFlash.transform.parent = players[_id].ARShootPoint;
        players[_id].ARMuzzleFlash = ClientChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
        ClientChildTempForMuzzleFlash = null;

        players[_id].ARCasePoint = FindGameObject(players[_id].AutomaticRifleModel, "ARCasePoint").transform;

        GameObject ClientChildTempForCasePoint = Instantiate(ARShellParticle.gameObject, players[_id].ARCasePoint.position, players[_id].ARCasePoint.rotation);
        ClientChildTempForCasePoint.transform.parent = players[_id].ARCasePoint;
        players[_id].ARBulletCasing = ClientChildTempForCasePoint.GetComponent<ParticleSystem>();
        ClientChildTempForCasePoint = null;
        #endregion

        #region MiniGun
        players[_id].MGShootPoint = FindGameObject(players[_id].gameObject, "MGShootPoint").transform;
        ClientChildTempForShootPoint = Instantiate(MGBulletParticle.gameObject, players[_id].MGShootPoint.position, players[_id].MGShootPoint.rotation);
        ClientChildTempForShootPoint.transform.parent = players[_id].MGShootPoint;
        players[_id].MGBulletParticle = ClientChildTempForShootPoint.GetComponent<ParticleSystem>();
        ClientChildTempForShootPoint = null;


        ClientChildTempForMuzzleFlash = Instantiate(MGMuzzleFlash.gameObject, players[_id].MGShootPoint.position, players[_id].MGShootPoint.rotation);
        ClientChildTempForMuzzleFlash.transform.parent = players[_id].MGShootPoint;
        players[_id].MGMuzzleFlash = ClientChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
        ChildTempForMuzzleFlash = null;

        players[_id].MGCasePoint = FindGameObject(players[_id].MiniGunModel, "MGCasePoint").transform;
        ClientChildTempForCasePoint = Instantiate(MGShellParticle.gameObject, players[_id].MGCasePoint.position, players[_id].MGCasePoint.rotation);
        ClientChildTempForCasePoint.transform.parent = players[_id].MGCasePoint;
        players[_id].MGBulletCasing = ClientChildTempForCasePoint.GetComponent<ParticleSystem>();
        ClientChildTempForCasePoint = null;
        #endregion

        #region Flame Thrower
        players[_id].FTShootPoint = FindGameObject(players[_id].gameObject, "FTShootPoint").transform;
        ClientChildTempForShootPoint = Instantiate(FTFlame.gameObject, players[_id].FTShootPoint.position, players[_id].FTShootPoint.rotation);
        ClientChildTempForShootPoint.transform.parent = players[_id].FTShootPoint;
        players[_id].FTFlame = ClientChildTempForShootPoint.GetComponent<ParticleSystem>();
        ClientChildTempForShootPoint = null;


        ClientChildTempForMuzzleFlash = Instantiate(FTHeatDistortion.gameObject, players[_id].FTShootPoint.position, players[_id].FTShootPoint.rotation);
        ClientChildTempForMuzzleFlash.transform.parent = players[_id].FTShootPoint;
        players[_id].FTHeatDistortion = ClientChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
        ClientChildTempForMuzzleFlash = null;

        #endregion

        #region ShotGun Starting
        //gets all the correct variables for the player controller by using prefabs and tags to put the correct things in the correct place
        GameObject[] ClientShotgunTempShootPoint = FindMultipleGameObject(players[_id].ShotGunModel, "SGShootPoint");
        players[_id].SGShootPoint = new Transform[ClientShotgunTempShootPoint.Length];
        players[_id].SGBulletParticle = new ParticleSystem[ClientShotgunTempShootPoint.Length];
        Debug.Log(ClientShotgunTempShootPoint.Length);
        for (int i = 0; i < ClientShotgunTempShootPoint.Length; i++)
        {
            Debug.Log(i);
            players[_id].SGShootPoint[i] = ClientShotgunTempShootPoint[i].transform;
            ClientChildTempForShootPoint = Instantiate(SGBulletParticle.gameObject, players[_id].SGShootPoint[i].position, players[_id].SGShootPoint[i].rotation);
            ClientChildTempForShootPoint.transform.parent = players[_id].SGShootPoint[i];
            players[_id].SGBulletParticle[i] = ClientChildTempForShootPoint.GetComponent<ParticleSystem>();
            ChildTempForShootPoint = null;
        }

        ClientChildTempForMuzzleFlash = Instantiate(ArMuzzleFlash.gameObject, players[_id].SGShootPoint[0].position, players[_id].SGShootPoint[0].rotation);
        ClientChildTempForMuzzleFlash.transform.parent = players[_id].SGShootPoint[0];
        players[_id].SGMuzzleFlash = ClientChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
        ChildTempForMuzzleFlash = null;
        players[_id].Health = 100;

        players[_id].SGCasePoint = FindGameObject(players[_id].ShotGunModel, "SGCasePoint").transform;
        ClientChildTempForCasePoint = Instantiate(ARShellParticle.gameObject, players[_id].SGCasePoint.position, players[_id].SGCasePoint.rotation);
        ClientChildTempForCasePoint.transform.parent = players[_id].SGCasePoint;
        players[_id].SGBulletCasing = ClientChildTempForCasePoint.GetComponent<ParticleSystem>();
        ClientChildTempForCasePoint = null;
        #endregion

        #region Side Arm Starting Initlisation
        //gets all the correct variables for the player controller by using prefabs and tags to put the correct things in the correct place
        players[_id].SAShootPoint = FindGameObject(players[_id].SideArmModel, "SAShootPoint").transform;
        ClientChildTempForShootPoint = Instantiate(ARBulletParticle.gameObject, players[_id].SAShootPoint.position, players[_id].SAShootPoint.rotation);
        ClientChildTempForShootPoint.transform.parent = players[_id].SAShootPoint;
        players[_id].SABulletParticle = ClientChildTempForShootPoint.GetComponent<ParticleSystem>();
        ClientChildTempForShootPoint = null;

        ClientChildTempForMuzzleFlash = Instantiate(ArMuzzleFlash.gameObject, players[_id].SAShootPoint.position, players[_id].SAShootPoint.rotation);
        ClientChildTempForMuzzleFlash.transform.parent = players[_id].SAShootPoint;
        players[_id].SAMuzzleFlash = ClientChildTempForMuzzleFlash.GetComponent<ParticleSystem>();
        ClientChildTempForMuzzleFlash = null;

        players[_id].SACasePoint = FindGameObject(players[_id].SideArmModel, "SACasePoint").transform;
        ClientChildTempForCasePoint = Instantiate(ARShellParticle.gameObject, players[_id].SACasePoint.position, players[_id].SACasePoint.rotation);
        ClientChildTempForCasePoint.transform.parent = players[_id].SACasePoint;
        players[_id].SABulletCasing = ClientChildTempForCasePoint.GetComponent<ParticleSystem>();
        ClientChildTempForCasePoint = null;
        #endregion
        NewWeaponValue(_id, 1);
    }
    public GameObject FindGameObject(GameObject PerentObject, string Tag) 
    {
        Transform[] ChildrenTrasform = PerentObject.GetComponentsInChildren<Transform>(); ;
        Transform FoundTransform = null;
        foreach (Transform ChildTransform in ChildrenTrasform)
        {
            if (ChildTransform.tag == Tag) 
            {
                FoundTransform = ChildTransform;
            }
        }
        return (FoundTransform.gameObject);
    }
    public GameObject[] FindMultipleGameObject(GameObject PerentObject, string Tag) 
    {
        Transform[] ChildrenTrasform = PerentObject.GetComponentsInChildren<Transform>();
        List<Transform> FoundTransformList = new List<Transform>();
        GameObject[] FoundTransformsArray;
        foreach (Transform ChildTransform in ChildrenTrasform)
        {
            if (ChildTransform.tag == Tag)
            {
                FoundTransformList.Add(ChildTransform);
            }
        }
        FoundTransformsArray = new GameObject[FoundTransformList.Count];

        for (int i = 0; i < FoundTransformsArray.Length; i++)
        {
            FoundTransformsArray[i] = FoundTransformList[i].gameObject;
        }

        return FoundTransformsArray;
    }

    public void NewWeaponValue(int ReceviedId, int NewWeaponValue)
    {
        players[ReceviedId].WeaponChange(NewWeaponValue);
    }
    public void ShootingServerRecevied(int ReceviedId, bool IsFiring)
    {
        players[ReceviedId].ShootingServerRecevied(IsFiring);
    }
}
