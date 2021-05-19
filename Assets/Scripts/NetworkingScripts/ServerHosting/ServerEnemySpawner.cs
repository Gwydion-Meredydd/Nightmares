using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEnemySpawner : MonoBehaviour
{
    public ScriptsManager SM;

    public Transform[] LevelSpawnPoints;

    public int InitalSpawnCap;

    public int CurrentSpawnCap;
    public int MaxSpawnCap;

    [Range(01, 4)]
    public float SpawnDelay;

    public bool IsSpawning;
    [Space]

    public int CurrentMonsterAmmount;

    public int SpawningAmmount;
    public int MaxSpawningAmmount;

    public int SpawnedMonsterAmmount;
    public int LevelMonsterAmmount;
    public GameObject[] MonsterPrefab;
    public GameObject[] GroundMonsterPrefab;
    void Update()
    {
        if (SM.HostingManager.SpawnLevel)
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
        if (SM.serverRoundManager.InActiveRound == true && SM.serverRoundManager.StartingNewRound == false)
        {
            if (IsSpawning == false)
            {
                IsSpawning = true;
                StartCoroutine(Spawning());
            }
        }
        else if (SM.serverRoundManager.StartingNewRound == false)
        {
            SM.serverRoundManager.StartCoroutine("NewRound");
            if (SpawningAmmount < MaxSpawningAmmount)
            {
                if (SM.serverRoundManager.RoundNumber < 4)
                {
                    SpawningAmmount = 2;
                }
                else
                {
                    if (SM.serverRoundManager.RoundNumber % 2 != 0)
                    {
                        Debug.Log("Odd number");
                        SpawningAmmount = (SM.serverRoundManager.RoundNumber - 1) / 2;
                    }
                }
            }
        }
    }
    IEnumerator Spawning()
    {
        yield return new WaitForSecondsRealtime(SpawnDelay);
        CurrentMonsterAmmount = SM.serverEnemyManager.ValueofEnemies.Length;
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
                    Transform SpawnPoint = LevelSpawnPoints[Random.Range(0, LevelSpawnPoints.Length)];
                    Vector3 SelectedSpawnPoint = SpawnPoint.position;
                    Quaternion QuaternionSpawning = new Quaternion(0, 0, 0, 0);
                    if (SpawnPoint.gameObject.layer == 23)
                    {
                        int randomValue = Random.Range(0, 3);
                        Instantiate(GroundMonsterPrefab[randomValue], SelectedSpawnPoint, QuaternionSpawning);
                        ServerSend.SpawnGroundEnemy(QuaternionSpawning, SelectedSpawnPoint, randomValue);
                    }
                    else
                    {
                        int randomValue = Random.Range(0, 3);
                        Instantiate(MonsterPrefab[randomValue], SelectedSpawnPoint, QuaternionSpawning);
                        ServerSend.SpawnNormalEnemy(QuaternionSpawning, SelectedSpawnPoint, randomValue);
                    }
                }
            }
            else
            {
                if (CurrentMonsterAmmount == 0)
                {
                    ServerPoints._serverPoints.RoundEndIncrease( 100);
                    SM.serverRoundManager.InActiveRound = false;
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
