using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEnemyDespawn : MonoBehaviour
{
    [HideInInspector]
    public static ServerEnemyDespawn _ServerEnemyDespawn;
    [HideInInspector]
    public ServerEnemyDespawn RefrenceServerEnemyDespawn;

    public GameObject[] Players;
    public bool PlayersFound;
    public float MaxDistance;
    [Space]

    public int MaxDeadEnemies;
    bool MassRemove;
    [HideInInspector]
    public GameObject[] DeadEnemiesValue;
    [HideInInspector]
    public List<GameObject> DeadEnemies;
    [HideInInspector]
    public List<Animator> DeadEnemiesAnimator;
    [HideInInspector]
    public List<Collider> DeadEnemiesCollider;
    [HideInInspector]
    public bool DeletingEnemy;
    private SkinnedMeshRenderer[] EnemieSkinnedMeshes;


    private void Start()
    {
        RefrenceServerEnemyDespawn = this;
        _ServerEnemyDespawn = RefrenceServerEnemyDespawn;
    }
    private void FixedUpdate()
    {
        if (Players.Length == 0 && !PlayersFound) 
        {
            Players = GameObject.FindGameObjectsWithTag("Player");
        }
        if (Players.Length > 0) 
        {
            PlayersFound = true;
        }
        if (DeadEnemiesValue.Length == MaxDeadEnemies)
        {
            if (MassRemove == false)
            {
                MassRemove = true;
                MassRemovalOfEnemies();
            }
        }
    }
    public void InstiantiateDespawn() 
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
    }
    private void LateUpdate()
    {
        if (MassRemove == false)
        {
            FetchDeadEnemies();
            DeadEnemiesDistanceDetection();
        }
    }
    void MassRemovalOfEnemies()
    {
        int OldAmmount = (DeadEnemiesValue.Length) / 4;
        for (int i = 0; i < OldAmmount; i++)
        {
            Destroy(DeadEnemiesValue[i]);
        }
        MassRemove = false;
    }
    void FetchDeadEnemies()
    {
        DeadEnemiesValue = GameObject.FindGameObjectsWithTag("DeadEnemy");
        DeadEnemies = new List<GameObject>(0);
        DeadEnemiesCollider = new List<Collider>(0);
        DeadEnemiesAnimator = new List<Animator>(0);
        if (DeadEnemies.Count < DeadEnemiesValue.Length)
        {
            int CountValue = 0;
            foreach (GameObject Enemies in GameObject.FindGameObjectsWithTag("DeadEnemy"))
            {
                DeadEnemies.Add(Enemies);
                CountValue = CountValue + 1;
            }
        }
    }
    void DeadEnemiesDistanceDetection()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        bool RemoveEnemy = false;
        int ArrayLength = 0;
        for (int i = 0; i < DeadEnemiesValue.Length; i++)
        {
            foreach (GameObject Player in Players)
            {
                if (Vector3.Distance(DeadEnemiesValue[i].transform.position, Player.transform.position) > MaxDistance) 
                {
                    RemoveEnemy = true;
                }
                else 
                {
                    RemoveEnemy = false;
                }
            }
            if (RemoveEnemy == true) 
            {
                Destroy(DeadEnemiesValue[ArrayLength]);
                RemoveEnemy = false;
            }
            ArrayLength = ArrayLength + 1;
        }
    }
}
