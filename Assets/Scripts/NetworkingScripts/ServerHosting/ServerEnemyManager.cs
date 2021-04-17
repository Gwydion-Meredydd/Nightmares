using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ServerEnemyManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    public int PointsDamage;
    public int PointsKill;
    [Space]
    public float AttackingDistance;
    public float AttackDamage;
    public float AttackCoolDown;
    public bool Attacking;
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

    void Update()
    {
        if (SM.HostingManager.SpawnLevel) 
        {
            if (AvailablePlayers.Length == 0)
            {
                AvailablePlayers = GameObject.FindGameObjectsWithTag("Player");
            }
            if (TakingDamage == false && EnemyDied == false && Attacking == false && HealthCalculation == false)
            {
                FetchActiveEnemies();
            }
            if (TakingDamage == true && Attacking == false)
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
                    ChasePlayer();
                    SendTransformToClients();
                }
                else
                {
                    ChaseRandom();
                }
            }
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
    void ChaseRandom()
    {
        //method that makes the ai chase random points around the map depending on the camera scripts dimensions
        //only if ignore player bool is active
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
                        RandomPosition = new Vector3(Random.Range(SM.CameraScript.maxPosition.x, SM.CameraScript.minPosition.x), 0, Random.Range(SM.CameraScript.maxPosition.z, SM.CameraScript.minPosition.z));
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
                            RandomPosition = new Vector3(Random.Range(SM.CameraScript.maxPosition.x, SM.CameraScript.minPosition.x), 0, Random.Range(SM.CameraScript.maxPosition.z, SM.CameraScript.minPosition.z));
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
                }
                ArrayLength = ArrayLength + 1;
            }
        }
    }
    GameObject ReturnClosestPlayer(Transform Enemy) 
    {
        GameObject ClosestPlayer = AvailablePlayers[0];
        switch (AvailablePlayers.Length) 
        {
            case 2:
                if (Vector3.Distance(AvailablePlayers[0].transform.position, Enemy.position) < Vector3.Distance(AvailablePlayers[1].transform.position, Enemy.position)) 
                {
                    ClosestPlayer = AvailablePlayers[0];
                }
                else 
                {
                    ClosestPlayer = AvailablePlayers[1];
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
    void ChasePlayer()
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
                float NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SelectedPlayer[ArrayLength].transform.position);
                if (NewDistance < AttackingDistance)
                {
                    if (Attacking == false)
                    {
                        Attacking = true;
                        StartCoroutine(AttackingTime(ArrayLength));
                    }
                }
            }
            ArrayLength = ArrayLength + 1;
        }
    }
    IEnumerator AttackingTime(int ArrayLength)
    {
        float NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SelectedPlayer[ArrayLength].transform.position);
        int OldNumberOfEnemies = ActiveEnemies.Count;
        if (NewDistance < AttackingDistance && Health[ArrayLength] > 0)
        {
            if (ActiveEnemiesAgents[ArrayLength].isActiveAndEnabled)
            {
                ActiveEnemiesAgents[ArrayLength].isStopped = true;
            }
            int AttackValue = Random.Range(1, 3);

//send Enemy attack animation here

            yield return new WaitForSecondsRealtime(0.1f);
            if (OldNumberOfEnemies == ActiveEnemies.Count)
            {
                NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SelectedPlayer[ArrayLength].transform.position);
                if (NewDistance < AttackingDistance)
                {
                    //Send Player Damage to HitPlayer
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
                        #region Color Damage
                        //instead of using color use quaternion to them caluculate on client side

                        Color newcolor = new Color(1, 1, 1, 1);
                        if (SM.PlayerScript.WeaponValue == 3)
                        {
                            if (Health[ArrayLength] < 10)
                            {
                                newcolor = new Color(0, 0, 0, 1);
                            }
                            else if (Health[ArrayLength] < 20)
                            {
                                newcolor = new Color(0.2f, 0.2f, 0.2f, 1);
                            }
                            else if (Health[ArrayLength] < 30)
                            {
                                newcolor = new Color(0.3f, 0.3f, 0.3f, 1);
                            }
                            else if (Health[ArrayLength] < 40)
                            {
                                newcolor = new Color(0.4f, 0.4f, 0.4f, 1);
                            }
                            else if (Health[ArrayLength] < 50)
                            {
                                newcolor = new Color(0.5f, 0.5f, 0.5f, 1);
                            }
                            else if (Health[ArrayLength] < 60)
                            {
                                newcolor = new Color(0.6f, 0.6f, 0.6f, 1);
                            }
                            else if (Health[ArrayLength] < 70)
                            {
                                newcolor = new Color(0.7f, 0.7f, 0.7f, 1);
                            }
                            else if (Health[ArrayLength] < 80)
                            {
                                newcolor = new Color(0.8f, 0.8f, 0.8f, 1);
                            }
                            else if (Health[ArrayLength] < 90)
                            {
                                newcolor = new Color(0.9f, 0.9f, 0.9f, 1);
                            }
                        }
                        else
                        {
                            if (Health[ArrayLength] < 10)
                            {
                                newcolor = new Color(1, 0.1f, 0.1f, 1);
                            }
                            else if (Health[ArrayLength] < 20)
                            {
                                newcolor = new Color(1, 0.2f, 0.2f, 1);
                            }
                            else if (Health[ArrayLength] < 30)
                            {
                                newcolor = new Color(1, 0.3f, 0.3f, 1);
                            }
                            else if (Health[ArrayLength] < 40)
                            {
                                newcolor = new Color(1, 0.4f, 0.4f, 1);
                            }
                            else if (Health[ArrayLength] < 50)
                            {
                                newcolor = new Color(1, 0.5f, 0.5f, 1);
                            }
                            else if (Health[ArrayLength] < 60)
                            {
                                newcolor = new Color(1, 0.6f, 0.6f, 1);
                            }
                            else if (Health[ArrayLength] < 70)
                            {
                                newcolor = new Color(1, 0.7f, 0.7f, 1);
                            }
                            else if (Health[ArrayLength] < 80)
                            {
                                newcolor = new Color(1, 0.8f, 0.8f, 1);
                            }
                            else if (Health[ArrayLength] < 90)
                            {
                                newcolor = new Color(1, 0.9f, 0.9f, 1);
                            }
                        }


                        // send new color data to clients

                        #endregion
                        if (Health[ArrayLength] <= 0)
                        {
                            int randomSpawnChance = Random.Range(1, 20);
                            if (randomSpawnChance == 10)
                            {
                                //drop a random drop
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
            EnemyHited = null;
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
                SelectedPlayer.Add(AvailablePlayers[Random.Range(0, AvailablePlayers.Length)]);
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
