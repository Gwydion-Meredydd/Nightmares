using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameManager GameManagerScript;
    public RoundManager RoundMangerScript;
    public EnemyManager EnemyManagerScript;
    public PointsManager PointScript;
    public PerksManager PerksScript;
    [Space]
    public Transform[] LevelSpawnPoints;
    [Space]
    public int InitalSpawnCap;
    public int CurrentSpawnCap;
    public int MaxSpawnCap;
    [Space]
    [Range(01, 4)]
    public float SpawnDelay;
    public bool IsSpawning;
    [Space]
    public int CurrentMonsterAmmount;
    public int SpawningAmmount;
    public int MaxSpawningAmmount;
    public int SpawnedMonsterAmmount;
    public int LevelMonsterAmmount;
    [Space]
    public GameObject MonsterPrefab;
    void Update()
    {
        if (GameManagerScript.InGame == true && GameManagerScript.Paused == false)
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
        if (RoundMangerScript.InActiveRound == true && RoundMangerScript.StartingNewRound == false)
        {
            if (IsSpawning == false)
            {
                IsSpawning = true;
                StartCoroutine(Spawning());
            }
        }
        else if (RoundMangerScript.StartingNewRound == false)
        {
            RoundMangerScript.StartCoroutine("NewRound");
            if (SpawningAmmount < MaxSpawningAmmount)
            {
                if (RoundMangerScript.RoundNumber < 4)
                {
                    SpawningAmmount = 2;
                }
                else
                {
                    if (RoundMangerScript.RoundNumber % 2 != 0)
                    {
                        Debug.Log("Odd number");
                        SpawningAmmount = (RoundMangerScript.RoundNumber - 1) / 2;
                    }
                }
            }
        }
    }
    IEnumerator Spawning() 
    {
        yield return new WaitForSecondsRealtime(SpawnDelay);
        CurrentMonsterAmmount = EnemyManagerScript.ValueofEnemies.Length;
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
                    PointScript.RoundEndIncrease(SpawningAmmount);
                    PerksScript.NewRound();
                    RoundMangerScript.InActiveRound = false;
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
