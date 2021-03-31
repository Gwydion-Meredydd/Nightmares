using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    public GameObject DeathScreen;
    public Image DeathFadeBlack;
    public bool PlayerIsDead;
    public bool Reset;
    private void LateUpdate()
    {
        if (Reset)
        {
            ResetScene();
            Reset = false;
        }
    }
    public void PlayerDead() 
    {
        if (!PlayerIsDead) 
        {
            StartCoroutine(ImageFadeToBlack());
            PlayerIsDead = true;
        }
    }
    IEnumerator ImageFadeToBlack()
    {
        int Score = Mathf.RoundToInt( SM.ScoreScript.Score);
        SM.ScoreScript.AddNewHighScore(SM.MainMenuScript.UserName, Score);
        DeathScreen.SetActive(true);
        if (SM.GameScript.Paused)
        {
            SM.PauseMenuManager.PauseToggle();
        }
        SM.FadeManager.FadeIn();
        yield return new  WaitForSecondsRealtime(1);
        foreach (var EnemyAudioSource in SM.EnemyScript.ActiveEnemiesAudioSources)
        {
            EnemyAudioSource.volume = 0;
        }
    }
    public void ResetStart() 
    {
        Reset = true;
    }
    public void ResetScene()
    {
        SM.GameScript.Paused = true;
        SM.GameMenuScript.MenuHasBeenInitilised = false;
        SM.MainMenuScript.HighScoreToggle();
        SM.GameScript.MainMenu.SetActive(true);
        SM.GameScript.GameMenu.SetActive(false);
        SM.EnemyScript.Health = new List<int>(0);
        SM.EnemyScript.ActiveEnemies = new List<GameObject>(0);
        SM.EnemyScript.ActiveEnemiesAgents = new List<UnityEngine.AI.NavMeshAgent>(0);
        SM.EnemyScript.ActiveEnemiesAnimators = new List<Animator>(0);
        SM.EnemyScript.ActiveEnemiesBoxColliders = new List<BoxCollider>(0);
        SM.EnemyScript.ActiveEnemiesAudioSources = new List<AudioSource>(0);
        SM.EnemyScript.EnemyInitilised = new List<bool>(0);
        SM.EnemyScript.HasReachedTarget = new bool[0];
        SM.EnemyScript.IgnorePlayer = false;
        foreach (var Enemy in SM.EnemyScript.ValueofEnemies)
        {
            Destroy(Enemy);
        }
        SM.EnemyScript.ValueofEnemies = new GameObject[0];
        SM.EnemyDespawn.DeadEnemies = new List<GameObject>(0);
        SM.EnemyDespawn.DeadEnemiesAnimator = new List<Animator>(0);
        SM.EnemyDespawn.DeadEnemiesCollider = new List<Collider>(0);
        foreach (var DeadEnemies in SM.EnemyDespawn.DeadEnemiesValue)
        {
            Destroy(DeadEnemies);
        }
        SM.EnemyDespawn.DeadEnemiesValue = new GameObject[0];
        SM.PlayerScript.PlayerAnimator = null;
        SM.PlayerScript.PlayerCc = null;
        SM.PlayerScript.Health = 0;
        SM.PlayerScript.AutomaticRifleModel = null;
        SM.PlayerScript.MiniGunModel = null;
        SM.PlayerScript.FlameThrowerModel = null;
        SM.PlayerScript.ShotGunModel = null;
        SM.PlayerScript.SideArmModel = null;
        SM.PlayerScript.ARShootPoint = null;
        SM.PlayerScript.MGShootPoint = null;
        SM.PlayerScript.FTShootPoint = null;
        SM.PlayerScript.SGShootPoint = new Transform[5];
        SM.PlayerScript.SAShootPoint = null;
        SM.PlayerScript.SACasePoint = null;
        SM.PlayerScript.WeaponAudioSource = null;
        SM.PlayerScript.PlayerAudioSource = null;
        SM.PlayerScript.ARBulletParticle = null;
        SM.PlayerScript.ARMuzzleFlash = null;
        SM.PlayerScript.ARBulletCasing = null;
        SM.PlayerScript.MGBulletParticle = null;
        SM.PlayerScript.MGMuzzleFlash = null;
        SM.PlayerScript.FTFlame = null;
        SM.PlayerScript.FTHeatDistortion = null;
        SM.PlayerScript.SGBulletParticle = new ParticleSystem[5];
        SM.PlayerScript.SGMuzzleFlash = null;
        SM.PlayerScript.SGBulletCasing = null;
        SM.PlayerScript.SABulletParticle = null;
        SM.PlayerScript.SAMuzzleFlash = null;
        SM.PlayerScript.SABulletCasing = null;
        SM.PlayerScript.Down = false;
        PlayerIsDead = false;
        Destroy(SM.PlayerScript.Player);
        SM.EnemySpawningScript.IsSpawning = false;
        SM.EnemySpawningScript.LevelSpawnPoints = new Transform[0];
        SM.EnemySpawningScript.CurrentMonsterAmmount = 0;
        SM.EnemySpawningScript.SpawningAmmount = 0;
        SM.EnemySpawningScript.SpawnedMonsterAmmount = 0;
        SM.EnemySpawningScript.CurrentSpawnCap = 0;
        SM.EnemySpawningScript.MonsterPrefab = new GameObject[0];
        SM.PerksScript.PerkValue = 0;
        SM.PerksScript.oldPerkValue = 0;
        SM.PerksScript.PerkSpawnPoint = null;
        SM.PerksScript.PerkSpawnVector = new Vector3(0,0,0);
        foreach(var PerkMachine in SM.PerksScript.PerkMachine) 
        {
            Destroy(PerkMachine);
        }
        SM.PerksScript.PerkMachine = new GameObject[0];
        SM.PerksScript.PerkMachineAnimator = new Animator[0];
        SM.PerksScript.PerkBaught = new bool[0];
        Destroy(SM.LevelScript.SpawnedLevel);
        SM.LevelScript.SpawnedLevel = null;
        SM.LevelScript.HeightOcclusionObjects = new GameObject[0];
        SM.LevelScript.HeightOcclusionReplacementObjects = new GameObject[0];
        SM.DeathScreenManager.DeathScreen.SetActive(false);
        SM.MainMenuScript.mainMenuObj.SetActive(true);
        SM.MainMenuScript.optionsMenuObj.SetActive(false);
        SM.ScoreScript.Score = 0;
        SM.PointsScript.Points = 0;
        SM.GameMenuScript.ScoreText.text = SM.ScoreScript.Score.ToString();
        SM.RoundScript.InActiveRound = false;
        SM.RoundScript.StartingNewRound = false;
        SM.RoundScript.RoundNumber = 0;
        SM.GameMenuScript.RoundText.text = SM.RoundScript.RoundNumber.ToString();
        SM.CoinScript.CoinAmmount = 3;
        SM.GameMenuScript.CoinMethod();
        SM.GameScript.InGame = false;
        SM.GameScript.Paused = false;
        SM.AudioScripts.MainMenuMusic.SetActive(true);
    }
}
