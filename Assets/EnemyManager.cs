using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameManager GameManagerScript;
    public PlayerController PlayerScript;
    public CameraController CameraScript;

    public List <bool> EnemyInitilised;
    public int StartingHealth;
    public List <int> Health;
    public bool IgnorePlayer;
    public bool InitalLost;
    public bool[] HasReachedTarget;
    public GameObject[] ActiveEnemies;
    public NavMeshAgent[] ActiveEnemiesAgents;

    public Vector3 RandomPosition;
    // Start is called before the first frame update
    void Update()
    {
        if (GameManagerScript.InGame == true) 
        {
            if (GameManagerScript.Paused == false) 
            {
                EnemyInitilisation();
                FetchActiveEnemies();
                if (IgnorePlayer == false) 
                {
                    if (InitalLost == true) 
                    {
                        InitalLost = false;
                    }
                    ChasePlayer();
                }
                else
                {
                    ChaseRandom();
                }
            }
        }
    }
    void EnemyInitilisation()
    {
        int ArrayLength = 0;
        int ListLength = 0;
        ArrayLength = ActiveEnemies.Length;
        ListLength = EnemyInitilised.Count;
        for (int i = 0; i < ListLength; i++)
        {
            if (EnemyInitilised[i] == false)
            {
                Debug.Log("1");
                Health[i] = StartingHealth;
                EnemyInitilised[i] = true;
            }
        }
        if (ListLength < ArrayLength)
        {
            Debug.Log("UnderHead");
            Health.Add(1);
            EnemyInitilised.Add(false);
        }
        else if (ListLength > ArrayLength) 
        {
            int Loopoverammount = ListLength - ArrayLength;
            for (int i = 0; i < Loopoverammount; i++)
            {
                ListLength = EnemyInitilised.Count;
                int RemovalValue = ListLength - 1;
                Debug.Log("OverHead");
                Health.RemoveAt(RemovalValue);
                EnemyInitilised.RemoveAt(RemovalValue);
            }
        }
    }
    void ChasePlayer()
    {
        int ArrayLength = 0;
        foreach (var GameObject in ActiveEnemiesAgents)
        {
            ActiveEnemiesAgents[ArrayLength].destination = PlayerScript.Player.transform.position;
            ArrayLength = ArrayLength + 1;
        }
    }
    void ChaseRandom() 
    {
        if (InitalLost == false)
        {
            InitalLost = true;
            int ArrayLength = 0;
            foreach (var GameObject in ActiveEnemiesAgents)
            {
                RandomPosition = new Vector3(Random.Range(CameraScript.maxPosition.x, CameraScript.minPosition.x), 0, Random.Range(CameraScript.maxPosition.z, CameraScript.minPosition.z));
                ActiveEnemiesAgents[ArrayLength].destination = RandomPosition;
                ArrayLength = ArrayLength + 1;
            }
        }
        else 
        {
            int ArrayLength = 0;
            foreach (var GameObject in ActiveEnemiesAgents) 
            {
                if (ActiveEnemiesAgents[ArrayLength].remainingDistance < 2) 
                {
                    RandomPosition = new Vector3(Random.Range(CameraScript.maxPosition.x, CameraScript.minPosition.x), 0, Random.Range(CameraScript.maxPosition.z, CameraScript.minPosition.z));
                    ActiveEnemiesAgents[ArrayLength].destination = RandomPosition;
                }
                ArrayLength = ArrayLength + 1;
            }
        }
    }
    void FetchActiveEnemies() 
    {
        ActiveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        int ArrayLength;
        ArrayLength = ActiveEnemies.Length;
        ActiveEnemiesAgents = new NavMeshAgent[ArrayLength];
        int TempEnemyValue = 0;
        foreach (var GameObject in ActiveEnemies)
        {
            ActiveEnemiesAgents[TempEnemyValue] = ActiveEnemies[TempEnemyValue].GetComponent<NavMeshAgent>(); ;
            TempEnemyValue = TempEnemyValue + 1;
        }
    }
}
