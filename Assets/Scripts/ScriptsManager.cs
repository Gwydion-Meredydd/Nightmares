using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptsManager : MonoBehaviour
{
    public GameManager GameScript;
    public PlayerController PlayerScript;
    public CameraController CameraScript;
    public EnemyManager EnemyScript;
    public EnemySpawner EnemySpawningScript;
    public EnemyDespawn EnemyDespawn;
    public PointsManager PointsScript;
    public PerksManager PerksScript;
    public RoundManager RoundScript;
    public DropsManager DropsScript;
    public AudioManager AudioScripts;
    public CoinManager CoinScript;
    public LevelManager LevelScript;
    public LocalNavMeshBuilder NavMeshAreaBuilder;
    public GameMenuManager GameMenuScript;
    public ScoreManager ScoreScript;
    public MainMenuManager MainMenuScript;
    public DeathScreenManager DeathScreenManager;
    public Fade FadeManager;
    public PauseMenuManager PauseMenuManager;
    public ClientManager Client_Manager;
}
