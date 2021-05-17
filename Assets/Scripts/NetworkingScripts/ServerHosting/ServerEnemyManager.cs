using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ServerEnemyManager : MonoBehaviour
{
    public ScriptsManager SM;
    public static ServerEnemyManager _serverEnemyManager;
    public ServerEnemyManager refrenceEnemyManger;

    [Space]
    public int PointsDamage;
    public int PointsKill;
    [Space]
    public float AttackingDistance;
    public float AttackDamage;
    public float AttackCoolDown;
    public bool Attacking;
    public int CurrentDamage;
    public int StartingHealth;
    public List<bool> EnemyInitilised;
    public List<int> Health;
    public bool HealthCalculation;
    public bool PauseMovement;
    public bool TakingDamage;
    public bool IgnorePlayer;
    public bool InitalLost;
    public bool[] HasReachedTarget;
    public GameObject EnemyHited;
    public bool EnemyDied;
    public GameObject[] ValueofEnemies;
    public List<GameObject> ActiveEnemies;
    public List<NavMeshAgent> ActiveEnemiesAgents;
    public List<BoxCollider> ActiveEnemiesBoxColliders;
    public List<GameObject> SelectedPlayer;
    public Vector3 RandomPosition;
    public GameObject[] AvailablePlayers;
    public bool ServerRestart;

    private void Start()
    {
        refrenceEnemyManger = this;
        _serverEnemyManager = refrenceEnemyManger;
    }
    private void FixedUpdate()
    {
        if (SM.HostingManager.SpawnLevel)
        {
            if (_serverEnemyManager == null)
            {
                Debug.Log("Instance not setting");
            }
            if (AvailablePlayers.Length == 0)
            {
                AvailablePlayers = GameObject.FindGameObjectsWithTag("Player");
                ServerPoints._serverPoints.Points = new int[AvailablePlayers.Length];
            }
            if (TakingDamage == false && EnemyDied == false && Attacking == false && HealthCalculation == false)
            {
                FetchActiveEnemies();
            }
            if (TakingDamage == true)
            {
                HealthManager();
            }
            if (TakingDamage == false && EnemyDied == false && Attacking == false && HealthCalculation == false && IgnorePlayer == false)
            {
                AttackPlayer();
            }
            if (HealthCalculation == false)
            {
                if (IgnorePlayer == false)
                {
                    if (InitalLost == true)
                    {
                        InitalLost = false;
                    }
                    ChoosePlayer();
                    SendTransformToClients();
                }
                else
                {
                    ChaseRandom();
                    SendTransformToClients();
                }
            }
            if (AvailablePlayers.Length == 0)
            {
                if (ServerRestart == false)
                {
                    Debug.Log("Server Restart");
                    ServerRestart = true;
                    StartCoroutine(ServerRestarting());
                }
            }
        }
    }
    IEnumerator ServerRestarting() 
    {
        yield return new WaitForSecondsRealtime(5);
        if (AvailablePlayers.Length == 0) 
        {
            Debug.Log("Server Restarting");
            int SCENEVALUE = SceneManager.GetActiveScene().buildIndex;
            ServerServer.Stop();
            yield return new WaitForSecondsRealtime(5);
            ServerServer.clients.Clear();
            SceneManager.LoadScene(1);
        }
        else 
        {
            ServerRestart = false;
        }
    }
    void SendTransformToClients()
    {
        Quaternion[] NewRotation = new Quaternion[ActiveEnemies.Count];
        Vector3[] NewPositions = new Vector3[ActiveEnemies.Count];
        for (int i = 0; i < ActiveEnemies.Count; i++)
        {
            NewRotation[i] = ActiveEnemies[i].transform.rotation;
            NewPositions[i] = ActiveEnemies[i].transform.position;
        }
        ServerSend.SendEnemyTransform(NewRotation, NewPositions);
    }
    public void ChaseRandom()
    {
        //method that makes the ai chase random points around the map depending on the camera scripts dimensions
        //only if ignore player bool is active
        IgnorePlayer = true;
        if (InitalLost == false)
        {
            int OldLength = ActiveEnemies.Count;
            InitalLost = true;
            for (int i = 0; i < OldLength; i++)
            {
                if (ActiveEnemiesAgents[i] != null)
                {
                    if (ActiveEnemiesAgents[i].isActiveAndEnabled)
                    {
                        RandomPosition = new Vector3(Random.Range(ServerHostingManager.Instance.MaxPosition.x, ServerHostingManager.Instance.MinPosition.x), 0, Random.Range(ServerHostingManager.Instance.MaxPosition.z, ServerHostingManager.Instance.MinPosition.z));
                        if (ActiveEnemiesAgents[i].isActiveAndEnabled)
                        {
                            ActiveEnemiesAgents[i].destination = RandomPosition;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            int OldLength = ActiveEnemies.Count;
            for (int i = 0; i < OldLength; i++)
            {
                if (OldLength == ActiveEnemies.Count)
                {
                    if (ActiveEnemiesAgents[i].isActiveAndEnabled)
                    {
                        if (ActiveEnemiesAgents[i].remainingDistance < 3 && ActiveEnemiesAgents[i] != null)
                        {
                            RandomPosition = new Vector3(Random.Range(ServerHostingManager.Instance.MaxPosition.x, ServerHostingManager.Instance.MinPosition.x), 0, Random.Range(ServerHostingManager.Instance.MaxPosition.z, ServerHostingManager.Instance.MinPosition.z));
                            ActiveEnemiesAgents[i].destination = RandomPosition;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
    void ChoosePlayer() 
    {
        if (HealthCalculation == false)
        {
            int ArrayLength = 0;
            foreach (var GameObject in ActiveEnemies)
            {
                if (ActiveEnemiesAgents[ArrayLength].isActiveAndEnabled)
                {
                    SelectedPlayer[ArrayLength] = ReturnClosestPlayer(ActiveEnemies[ArrayLength].transform);
                    if (SelectedPlayer[ArrayLength] != null) 
                    {
                        SelectedPlayer[ArrayLength] = null;
                        SelectedPlayer[ArrayLength] = ReturnClosestPlayer(ActiveEnemies[ArrayLength].transform);
                        ChasePlayer();
                    }
                    else
                    {
                        ChaseRandom();
                    }
                    
                }
                ArrayLength = ArrayLength + 1;
            }
        }
    }
    public GameObject ReturnClosestPlayer(Transform Enemy)
    {
        AvailablePlayers = GameObject.FindGameObjectsWithTag("Player");
        if (AvailablePlayers.Length > 0)
        {
            GameObject ClosestPlayer = AvailablePlayers[0];

            switch (AvailablePlayers.Length)
            {
                case 2:
                    if (Vector3.Distance(AvailablePlayers[0].transform.position, Enemy.position) < Vector3.Distance(AvailablePlayers[1].transform.position, Enemy.position))
                    {
                        if (!AvailablePlayers[0].GetComponent<ServerPlayer>().PlayerDown)
                        {
                            ClosestPlayer = AvailablePlayers[0];
                        }
                        else if (!AvailablePlayers[1].GetComponent<ServerPlayer>().PlayerDown)
                        {
                            ClosestPlayer = AvailablePlayers[1];
                        }
                        else
                        {
                            ClosestPlayer = null;
                        }
                    }
                    else if (Vector3.Distance(AvailablePlayers[1].transform.position, Enemy.position) < Vector3.Distance(AvailablePlayers[0].transform.position, Enemy.position))
                    {
                        if (!AvailablePlayers[1].GetComponent<ServerPlayer>().PlayerDown)
                        {
                            ClosestPlayer = AvailablePlayers[1];
                        }
                        else if (!AvailablePlayers[0].GetComponent<ServerPlayer>().PlayerDown)
                        {
                            ClosestPlayer = AvailablePlayers[0];
                        }
                        else
                        {
                            ClosestPlayer = null;
                        }
                    }
                    break;
                case 3:
                    if (Vector3.Distance(AvailablePlayers[0].transform.position, Enemy.position) < Vector3.Distance(AvailablePlayers[1].transform.position, Enemy.position))
                    {
                        ClosestPlayer = AvailablePlayers[0];
                    }
                    else if (Vector3.Distance(AvailablePlayers[1].transform.position, Enemy.position) < Vector3.Distance(AvailablePlayers[2].transform.position, Enemy.position))
                    {
                        ClosestPlayer = AvailablePlayers[1];
                    }
                    else
                    {
                        ClosestPlayer = AvailablePlayers[2];
                    }
                    break;
                case 4:
                    if (Vector3.Distance(AvailablePlayers[0].transform.position, Enemy.position) < Vector3.Distance(AvailablePlayers[1].transform.position, Enemy.position))
                    {
                        ClosestPlayer = AvailablePlayers[0];
                    }
                    else if (Vector3.Distance(AvailablePlayers[1].transform.position, Enemy.position) < Vector3.Distance(AvailablePlayers[2].transform.position, Enemy.position))
                    {
                        ClosestPlayer = AvailablePlayers[1];
                    }
                    else if (Vector3.Distance(AvailablePlayers[2].transform.position, Enemy.position) < Vector3.Distance(AvailablePlayers[3].transform.position, Enemy.position))
                    {
                        ClosestPlayer = AvailablePlayers[2];
                    }
                    else
                    {
                        ClosestPlayer = AvailablePlayers[3];
                    }
                    break;

            }
            return (ClosestPlayer);
        }
        else 
        {
            return (null);
        }
    }
    public void ChasePlayer()
    {
        //The method that sets all enemeies to chase the player 
        //only is used if the ingore player bool is active
        if (HealthCalculation == false)
        {
            int ArrayLength = 0;
            foreach (var GameObject in ActiveEnemies)
            {
                if (ActiveEnemiesAgents[ArrayLength].isActiveAndEnabled)
                {
                    ActiveEnemiesAgents[ArrayLength].destination = SelectedPlayer[ArrayLength].transform.position;
                }
                ArrayLength = ArrayLength + 1;
            }
        }
    }
    public void AttackPlayer()
    {
        int ArrayLength = 0;
        foreach (var GameObject in ActiveEnemies)
        {
            if (Health[ArrayLength] > 0)
            {
                if (SelectedPlayer[ArrayLength] != null)
                {
                    float NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SelectedPlayer[ArrayLength].transform.position);
                    if (NewDistance < AttackingDistance)
                    {
                        if (Attacking == false && SelectedPlayer[ArrayLength] != null)
                        {
                            Attacking = true;
                            StartCoroutine(AttackingTime(ArrayLength));
                        }
                    }
                }
            }
            ArrayLength = ArrayLength + 1;
        }
    }
    IEnumerator AttackingTime(int ArrayLength)
    {
        if (SelectedPlayer[ArrayLength] != null)
        {
            ServerPlayer sPlayer = SelectedPlayer[ArrayLength].GetComponent<ServerPlayer>();
            if (sPlayer.Health > 0) 
            {
                float NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SelectedPlayer[ArrayLength].transform.position);
                int OldNumberOfEnemies = ActiveEnemies.Count;
                if (NewDistance < AttackingDistance && Health[ArrayLength] > 0)
                {
                    if (ActiveEnemiesAgents[ArrayLength].isActiveAndEnabled)
                    {
                        ActiveEnemiesAgents[ArrayLength].isStopped = true;
                    }

                    ServerSend.SendEnemyAttack(ArrayLength);

                    yield return new WaitForSecondsRealtime(0.1f);
                    if (SelectedPlayer[ArrayLength] != null)
                    {
                        if (OldNumberOfEnemies == ActiveEnemies.Count)
                        {
                            NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SelectedPlayer[ArrayLength].transform.position);
                            if (NewDistance < AttackingDistance)
                            {
                                int PlayerID = SelectedPlayer[ArrayLength].GetComponent<ServerPlayer>().id;
                                SelectedPlayer[ArrayLength].GetComponent<ServerPlayer>().Health = SelectedPlayer[ArrayLength].GetComponent<ServerPlayer>().Health - AttackDamage;
                                float PlayerHealth = SelectedPlayer[ArrayLength].GetComponent<ServerPlayer>().Health;
                                SelectedPlayer[ArrayLength].GetComponent<ServerPlayer>().HealthCalculations();
                                ServerSend.SendEnemyHitPlayer(PlayerID, PlayerHealth);
                                yield return new WaitForSecondsRealtime(0.1f);
                                if (OldNumberOfEnemies == ActiveEnemies.Count)
                                {
                                    //Send Player Damage to HitPlayer off
                                }
                            }
                            if (OldNumberOfEnemies == ActiveEnemies.Count)
                            {
                                yield return new WaitForSecondsRealtime(AttackCoolDown);
                                if (OldNumberOfEnemies == ActiveEnemies.Count)
                                {
                                    ActiveEnemiesAgents[ArrayLength].isStopped = false;

                                    //send enemy move here

                                }
                            }
                        }
                    }
                }
            }
            else 
            {
                ChaseRandom();
            }
        }
        Attacking = false;
    }
    public void HealthManager()
    {
        if (HealthCalculation == false)
        {
            HealthCalculation = true;
            int ArrayLength = 0;
            foreach (var GameObject in ActiveEnemies)
            {
                GameObject TemporaryActiveEnemie = ActiveEnemies[ArrayLength];
                if (Health[ArrayLength] != 0)
                {
                    if (EnemyHited == TemporaryActiveEnemie)
                    {
                        ActiveEnemiesAgents[ArrayLength].speed = 0;
                        int RandomDamageValue = Random.Range(1, 4);
                        switch (RandomDamageValue)
                        {
                            case 1:
                                // Send Enemy Hit Type 1
                                break;
                            case 2:
                                // Send Enemy Hit Type 2
                                break;
                            case 3:
                                // Send Enemy Hit Type 3
                                break;
                        }

                        //add points

                        // send new player health to client
                        Health[ArrayLength] = Health[ArrayLength] - CurrentDamage;
                        ServerSend.SendEnemyDamage(ArrayLength, Health[ArrayLength]);
                        CurrentDamage = 0;
                        if (Health[ArrayLength] <= 0)
                        {
                            int randomSpawnChance = Random.Range(1, 20);
                            if (randomSpawnChance == 10)
                            {
                                ServerDropManager._serverDropManager.DropRandom(ActiveEnemies[ArrayLength].transform.position);
                            }
                            
                            // send kill points to client
                            //ActiveEnemiesAnimators[ArrayLength].SetBool("Hit", false);
                            //ActiveEnemiesAnimators[ArrayLength].SetBool("Dead", true);
                            //ActiveEnemiesAnimators[ArrayLength].SetBool("Attack", false);
                            //int DeathValue = Random.Range(1, 4);
                            //ActiveEnemiesAnimators[ArrayLength].SetInteger("DeathRandomiser", DeathValue);
                            //send Enemy Death with random death value 
                            if (ActiveEnemiesAgents[ArrayLength].isActiveAndEnabled)
                            {
                                ActiveEnemiesAgents[ArrayLength].isStopped = true;
                            }
                            else
                            {
                                ActiveEnemiesAgents[ArrayLength].enabled = true;
                                ActiveEnemiesAgents[ArrayLength].isStopped = true;
                            }
                            ActiveEnemiesBoxColliders[ArrayLength].enabled = false;
                            ActiveEnemies[ArrayLength].tag = "DeadEnemy";
                            ActiveEnemies.RemoveAt(ArrayLength);
                            Health.RemoveAt(ArrayLength);
                            EnemyInitilised.RemoveAt(ArrayLength);
                            ActiveEnemiesAgents.RemoveAt(ArrayLength);
                            SelectedPlayer.RemoveAt(ArrayLength);
                            break;
                        }
                        ActiveEnemiesAgents[ArrayLength].speed = 3.5f;
                    }
                }
                ArrayLength = ArrayLength + 1;
            }
            //EnemyHited = null;
            TakingDamage = false;
            HealthCalculation = false;
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
            int ArrayLength;
            ArrayLength = ActiveEnemies.Count;
            ActiveEnemiesAgents = new List<NavMeshAgent>(ArrayLength);
            SelectedPlayer = new List<GameObject>(ArrayLength);
            ActiveEnemiesBoxColliders = new List<BoxCollider>(ArrayLength);
            foreach (GameObject Enemies in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                ActiveEnemiesAgents.Add(Enemies.GetComponent<NavMeshAgent>());
                ActiveEnemiesBoxColliders.Add(Enemies.GetComponent<BoxCollider>());
                SelectedPlayer.Add(ReturnClosestPlayer(Enemies.transform));
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
                        ActiveEnemiesAgents[i].speed = Random.Range(3, 4);
                    }
                }
            }
        }
    }
}
