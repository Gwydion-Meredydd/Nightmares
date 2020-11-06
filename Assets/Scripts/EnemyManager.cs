using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameManager GameManagerScript;
    public PlayerController PlayerScript;
    public CameraController CameraScript;

    public List<bool> EnemyInitilised;
    public int StartingHealth;
    public List<int> Health;
    public bool HealthCalculation;
    public bool TakingDamage;
    public bool IgnorePlayer;
    public bool InitalLost;
    public bool[] HasReachedTarget;

    public GameObject EnemyHited;
    public bool EnemyDied;
    public GameObject[] ValueofEnemies;
    public List<GameObject> ActiveEnemies;
    public List<NavMeshAgent> ActiveEnemiesAgents;
    public List<Animator> ActiveEnemiesAnimators;
    public List<BoxCollider> ActiveEnemiesBoxColliders;


    public Vector3 RandomPosition;
    // Start is called before the first frame update
    void Update()
    {
        if (GameManagerScript.InGame == true)
        {
            if (GameManagerScript.Paused == false)
            {
                if (TakingDamage == false && EnemyDied == false)
                {
                    FetchActiveEnemies();
                }
                if (TakingDamage == true)
                {
                    HealthManager();
                    TakingDamage = false;
                }
                if (EnemyDied == false)
                {
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
    }
    public void HealthManager()
    {
        Debug.Log("1");
        if (HealthCalculation == false)
        {
            HealthCalculation = true;
            int ArrayLength = 0;
            foreach (var GameObject in ValueofEnemies)
            {
                GameObject TemporaryActiveEnemie = ValueofEnemies[ArrayLength];
                if (Health[ArrayLength] != 0)
                {
                    if (EnemyHited == TemporaryActiveEnemie)
                    {
                        Debug.Log("HealthShot");
                        Health[ArrayLength] = Health[ArrayLength] - PlayerScript.CurrentDamage;
                        if (Health[ArrayLength] <= 0)
                        {
                            Debug.Log("Remove Health");
                            Health.RemoveAt(ArrayLength);
                            EnemyInitilised.RemoveAt(ArrayLength);
                            ActiveEnemiesAnimators[ArrayLength].SetBool("Dead", true);
                            int DeathValue = Random.Range(1, 4);
                            Debug.Log(DeathValue);
                            ActiveEnemiesAnimators[ArrayLength].SetInteger("DeathRandomiser", DeathValue);
                            ActiveEnemiesAgents[ArrayLength].isStopped = true;
                            ActiveEnemiesBoxColliders[ArrayLength].enabled = false;
                            //Destroy(ActiveEnemies[ArrayLength]);
                            EnemyDied = true;
                            StartCoroutine("EnemyDeathCooldown");
                            break;
                        }
                    }
                }
                ArrayLength = ArrayLength + 1;
            }
            EnemyHited = null;
        }
        HealthCalculation = false;

    }
    void ChasePlayer()
    {
        if (HealthCalculation == false)
        {
            int ArrayLength = 0;
            foreach (var GameObject in ValueofEnemies)
            {
                ActiveEnemiesAgents[ArrayLength].destination = PlayerScript.Player.transform.position;
                ArrayLength = ArrayLength + 1;
            }
        }
    }
    void ChaseRandom()
    {
        if (InitalLost == false)
        {
            int OldLength = ValueofEnemies.Length;
            InitalLost = true;
            for (int i = 0; i < OldLength; i++)
            {
                if (ActiveEnemiesAgents[i] != null)
                {
                    RandomPosition = new Vector3(Random.Range(CameraScript.maxPosition.x, CameraScript.minPosition.x), 0, Random.Range(CameraScript.maxPosition.z, CameraScript.minPosition.z));
                    ActiveEnemiesAgents[i].destination = RandomPosition;
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            int OldLength = ValueofEnemies.Length;
            for (int i = 0; i < OldLength; i++)
            {
                if (OldLength == ValueofEnemies.Length)
                {
                    if (ActiveEnemiesAgents[i].remainingDistance < 2 && ActiveEnemiesAgents[i] != null)
                    {
                        RandomPosition = new Vector3(Random.Range(CameraScript.maxPosition.x, CameraScript.minPosition.x), 0, Random.Range(CameraScript.maxPosition.z, CameraScript.minPosition.z));
                        ActiveEnemiesAgents[i].destination = RandomPosition;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
    public void FetchActiveEnemies()
    {
        if (TakingDamage == false)
        {
            ValueofEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            ActiveEnemies = new List<GameObject>(0);
            if (ActiveEnemies.Count < ValueofEnemies.Length)
            {
                foreach (GameObject Enemies in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    ActiveEnemies.Add(Enemies);
                }
            }
            //ActiveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            int ArrayLength;
            ArrayLength = ActiveEnemies.Count;
            ActiveEnemiesAgents = new List<NavMeshAgent>(ArrayLength);
            ActiveEnemiesAnimators = new List<Animator>(ArrayLength);
            ActiveEnemiesBoxColliders = new List<BoxCollider>(ArrayLength);
            foreach (GameObject Enemies in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                ActiveEnemiesAgents.Add(Enemies.GetComponent<NavMeshAgent>());
                ActiveEnemiesAnimators.Add(Enemies.GetComponent<Animator>());
                ActiveEnemiesBoxColliders.Add(Enemies.GetComponent<BoxCollider>());
            }
            ValueofEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (Health.Count <= ValueofEnemies.Length)
            {
                for (int i = 0; i < ValueofEnemies.Length; i++)
                {
                    if (Health.Count == ValueofEnemies.Length || TakingDamage == true)
                    {
                        break;
                    }
                    if (ValueofEnemies.Length != Health.Count && TakingDamage == false)
                    {
                        Debug.Log("Adding Health");
                        Health.Add(1);
                        EnemyInitilised.Add(false);
                    }
                }
                for (int i = 0; i < ValueofEnemies.Length; i++)
                {
                    if (EnemyInitilised[i] == false)
                    {
                        Health[i] = StartingHealth;
                        EnemyInitilised[i] = true;
                    }
                }
            }
        }
    }
    IEnumerator EnemyDeathCooldown()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        EnemyDied = false;
    }
}

