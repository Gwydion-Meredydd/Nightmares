using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ScriptsManager SM;
    [HideInInspector]
    public Transform[] LevelSpawnPoints;
    [Space]
    public int InitalSpawnCap;
    [HideInInspector]
    public int CurrentSpawnCap;
    public int MaxSpawnCap;
    [Space]
    [Range(01, 4)]
    public float SpawnDelay;
    [HideInInspector]
    public bool IsSpawning;
    [Space]
    [HideInInspector]
    public int CurrentMonsterAmmount;
    [HideInInspector]
    public int SpawningAmmount;
    public int MaxSpawningAmmount;
    [HideInInspector]
    public int SpawnedMonsterAmmount;
    public int LevelMonsterAmmount;
    [HideInInspector]
    public GameObject MonsterPrefab;
    void Update()
    {
        if (SM.GameScript.InGame == true && SM.GameScript.Paused == false)
        {
            SpawnMethod();
        }
    }
    public void SpawnMethod() 
    {
        if (CurrentSpawnCap == 0) 
        {
            CurrentSpawnCap = InitalSpawnCap;
        }
        if (SM.RoundScript.InActiveRound == true && SM.RoundScript.StartingNewRound == false)
        {
            if (IsSpawning == false)
            {
                IsSpawning = true;
                StartCoroutine(Spawning());
            }
        }
        else if (SM.RoundScript.StartingNewRound == false)
        {
            SM.RoundScript.StartCoroutine("NewRound");
            if (SpawningAmmount < MaxSpawningAmmount)
            {
                if (SM.RoundScript.RoundNumber < 4)
                {
                    SpawningAmmount = 2;
                }
                else
                {
                    if (SM.RoundScript.RoundNumber % 2 != 0)
                    {
                        Debug.Log("Odd number");
                        SpawningAmmount = (SM.RoundScript.RoundNumber - 1) / 2;
                    }
                }
            }
        }
    }
    IEnumerator Spawning() 
    {
        yield return new WaitForSecondsRealtime(SpawnDelay);
        CurrentMonsterAmmount = SM.EnemyScript.ValueofEnemies.Length;
        if (CurrentMonsterAmmount < CurrentSpawnCap) 
        {
            if (SpawnedMonsterAmmount < LevelMonsterAmmount) 
            {
                for (int SpawnLoopValue = 0; SpawnLoopValue < SpawningAmmount; SpawnLoopValue++)
                {
                    if (SpawnedMonsterAmmount >= LevelMonsterAmmount) 
                    {
                        break;
                    }
                    SpawnedMonsterAmmount = SpawnedMonsterAmmount + 1;
                    Vector3 SelectedSpawnPoint = LevelSpawnPoints[Random.Range(0, LevelSpawnPoints.Length)].position;
                    Quaternion QuaternionSpawning = new Quaternion(0, 0, 0, 0);
                    Instantiate(MonsterPrefab, SelectedSpawnPoint, QuaternionSpawning);
                }
            }
            else 
            {
                if (CurrentMonsterAmmount == 0)
                {
                    SM.PointsScript.RoundEndIncrease(SpawningAmmount);
                    SM.PerksScript.NewRound();
                    SM.RoundScript.InActiveRound = false;
                    SpawnedMonsterAmmount = 0;
                    LevelMonsterAmmount = LevelMonsterAmmount + 6;
                    if (CurrentSpawnCap < MaxSpawnCap)
                    {
                        CurrentSpawnCap = CurrentSpawnCap + 4;
                    }
                }
            }
        }
        IsSpawning = false;
    }

}
