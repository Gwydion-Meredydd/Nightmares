using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    public int PointsDamage;
    public int PointsKill;
    [Space]
    public float AttackingDistance;
    public float AttackDamage;
    public float AttackCoolDown;
    [HideInInspector]
    public bool Attacking;
    public int StartingHealth;
    [HideInInspector]
    public List<bool> EnemyInitilised;
    [HideInInspector]
    public List<int> Health;
    [HideInInspector]
    public bool HealthCalculation;
    [HideInInspector]
    public bool TakingDamage;
    public bool IgnorePlayer;
    [HideInInspector]
    public bool InitalLost;
    [HideInInspector]
    public bool[] HasReachedTarget;
    [HideInInspector]
    public GameObject EnemyHited;
    [HideInInspector]
    public bool EnemyDied;
    [HideInInspector]
    public GameObject[] ValueofEnemies;
    [HideInInspector]
    public List<GameObject> ActiveEnemies;
    [HideInInspector]
    public List<NavMeshAgent> ActiveEnemiesAgents;
    [HideInInspector]
    public List<Animator> ActiveEnemiesAnimators;
    [HideInInspector]
    public List<BoxCollider> ActiveEnemiesBoxColliders;
    [HideInInspector]
    public List<AudioSource> ActiveEnemiesAudioSources;
    [HideInInspector]
    public Vector3 RandomPosition;
    // Start is called before the first frame update
    void Update()
    {
        //Checks if the game is running and if the game is paused or not and then calls the required methods to opprate the enemies (also some methods aren't called when others are active for performance)
        if (SM.GameScript.InGame == true)
        {
            if (SM.GameScript.Paused == false)
            {
                if (TakingDamage == false && EnemyDied == false && Attacking == false && HealthCalculation == false)
                {
                    FetchActiveEnemies();
                }
                if (TakingDamage == true && Attacking == false)
                {
                    HealthManager();
                    //TakingDamage = false;
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
        //method is called when player's raycast hit any enemy, 
        //enemy is known by setting enemy hitted with that raycast object
        //removes enmy tag from that enemy making it not valid for script interaction
        //removes health etc 
        //random death animaiton out of 3
        //removes that enemy from the lists and arrays at the number that the player shot at if the health was at 0
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
                                ActiveEnemiesAnimators[ArrayLength].Play("Hit 1", 2);
                                SM.PlayerScript.PlayerAudioSource.PlayOneShot(SM.AudioScripts.EnemyHitMarkers[0]);
                                break;
                            case 2:
                                ActiveEnemiesAnimators[ArrayLength].Play("Hit 2", 2);
                                SM.PlayerScript.PlayerAudioSource.PlayOneShot(SM.AudioScripts.EnemyHitMarkers[1]);
                                break;
                            case 3:
                                ActiveEnemiesAnimators[ArrayLength].Play("Hit 3", 2);
                                SM.PlayerScript.PlayerAudioSource.PlayOneShot(SM.AudioScripts.EnemyHitMarkers[2]);
                                break;
                        }
                        SM.PointsScript.PointsIncrease(PointsDamage);
                        Health[ArrayLength] = Health[ArrayLength] - SM.PlayerScript.CurrentDamage;
                        if (Health[ArrayLength] <= 0)
                        {
                            int randomSpawnChance = Random.Range(1, 20);
                            if (randomSpawnChance == 10)
                            {
                                SM.DropsScript.DropRandom(ActiveEnemies[ArrayLength].transform.position);
                            }
                            SM.PointsScript.PointsIncrease(PointsKill);
                            ActiveEnemiesAnimators[ArrayLength].SetBool("Hit", false);
                            ActiveEnemiesAnimators[ArrayLength].SetBool("Dead", true);
                            ActiveEnemiesAnimators[ArrayLength].SetBool("Attack", false);
                            //Attacking = false;
                            int DeathValue = Random.Range(1, 4);
                            ActiveEnemiesAnimators[ArrayLength].SetInteger("DeathRandomiser", DeathValue);
                            ActiveEnemiesAgents[ArrayLength].isStopped = true;
                            ActiveEnemiesBoxColliders[ArrayLength].enabled = false;

                            //EnemyDied = true;
                            ActiveEnemies[ArrayLength].tag = "DeadEnemy";
                            ActiveEnemies.RemoveAt(ArrayLength);
                            Health.RemoveAt(ArrayLength);
                            EnemyInitilised.RemoveAt(ArrayLength);
                            ActiveEnemiesAgents.RemoveAt(ArrayLength);
                            ActiveEnemiesAnimators.RemoveAt(ArrayLength);
                            ActiveEnemiesBoxColliders.RemoveAt(ArrayLength);
                            ActiveEnemiesAudioSources.RemoveAt(ArrayLength);
                            //StartCoroutine("EnemyDeathCooldown");
                            break;
                        }
                        ActiveEnemiesAgents[ArrayLength].speed = 3.5f;
                        //ActiveEnemiesAnimators[ArrayLength].SetBool("Hit", false);
                    }
                }
                ArrayLength = ArrayLength + 1;
            }
            EnemyHited = null;
            TakingDamage = false;
            HealthCalculation = false;
        }

    }
    public void AttackPlayer()
    {
        //Called when the enemy script isnt taking any damange
        //checks if any active enemies is in range of the player
        //only checks enemies that have more than 0 health
        //starts a coroutine with the array number (enemy number) as a peramenter
        int ArrayLength = 0;
        foreach (var GameObject in ActiveEnemies)
        {
            if (Health[ArrayLength] > 0)
            {
                float NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SM.PlayerScript.Player.transform.position);
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
        //timing for enemy attacking players
        // will allways swing arm of the enemy (randomised out of 2)
        //checks if the player is in range of the attacking enemy when the arm swings
        //applies damage to player if in range on swing and starts player damage attack and camera effect
        float NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SM.PlayerScript.Player.transform.position);
        int OldNumberOfEnemies = ActiveEnemies.Count;
        if (NewDistance < AttackingDistance && Health[ArrayLength] > 0)
        { 
            ActiveEnemiesAgents[ArrayLength].isStopped = true;
            int AttackValue = Random.Range(1, 3);
            ActiveEnemiesAnimators[ArrayLength].SetInteger("AttackRandomiser", AttackValue);
            ActiveEnemiesAnimators[ArrayLength].SetBool("Moving", false);
            ActiveEnemiesAnimators[ArrayLength].SetBool("Attack", true);
            ActiveEnemiesAudioSources[ArrayLength].PlayOneShot(SM.AudioScripts.EnemyAttacking[(Random.Range(0, 3))]);

            yield return new WaitForSecondsRealtime(0.1f);
            if (OldNumberOfEnemies == ActiveEnemies.Count)
            {
                NewDistance = Vector3.Distance(ActiveEnemies[ArrayLength].transform.position, SM.PlayerScript.Player.transform.position);
                if (NewDistance < AttackingDistance)
                {
                    SM.PlayerScript.PlayerAnimator.SetBool("Hurt", true);

                    SM.PlayerScript.Health = SM.PlayerScript.Health - AttackDamage;
                    SM.GameMenuScript.HealthMethod(SM.PlayerScript.Health);
                    if (SM.GameScript.PlayerTypeValue == 3 || SM.GameScript.PlayerTypeValue == 4)
                    {
                        SM.PlayerScript.PlayerAudioSource.PlayOneShot(SM.AudioScripts.PlayerHurtMale[(Random.Range(0, 3))]);
                    }
                    if (SM.GameScript.PlayerTypeValue == 1 || SM.GameScript.PlayerTypeValue == 2)
                    {
                        SM.PlayerScript.PlayerAudioSource.PlayOneShot(SM.AudioScripts.PlayerHurtFemale[(Random.Range(0, 3))]);
                    }
                    SM.PlayerScript.CameraDamage();
                    yield return new WaitForSecondsRealtime(0.1f);
                    if (OldNumberOfEnemies == ActiveEnemies.Count)
                    {
                        SM.CameraScript.yValue = SM.CameraScript.HoldingYValue;
                        SM.PlayerScript.PlayerAnimator.SetBool("Hurt", false);
                    }
                }
                if (OldNumberOfEnemies == ActiveEnemies.Count)
                {
                    yield return new WaitForSecondsRealtime(AttackCoolDown);
                    if (OldNumberOfEnemies == ActiveEnemies.Count)
                    {
                        ActiveEnemiesAgents[ArrayLength].isStopped = false;
                        ActiveEnemiesAnimators[ArrayLength].SetBool("Moving", true);
                        ActiveEnemiesAnimators[ArrayLength].SetBool("Attack", false);
                    }
                }
            }
        }
        Attacking = false;
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
                ActiveEnemiesAgents[ArrayLength].destination = SM.PlayerScript.Player.transform.position;
                ArrayLength = ArrayLength + 1;
            }
        }
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
                    RandomPosition = new Vector3(Random.Range(SM.CameraScript.maxPosition.x, SM.CameraScript.minPosition.x), 0, Random.Range(SM.CameraScript.maxPosition.z, SM.CameraScript.minPosition.z));
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
            int OldLength = ActiveEnemies.Count;
            for (int i = 0; i < OldLength; i++)
            {
                if (OldLength == ActiveEnemies.Count)
                {
                    if (ActiveEnemiesAgents[i].remainingDistance < 3 && ActiveEnemiesAgents[i] != null)
                    {
                        RandomPosition = new Vector3(Random.Range(SM.CameraScript.maxPosition.x, SM.CameraScript.minPosition.x), 0, Random.Range(SM.CameraScript.maxPosition.z, SM.CameraScript.minPosition.z));
                        ActiveEnemiesAgents[i].destination = RandomPosition;
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
        //Method that controls all the lists of needed component and objects for control of enemies
        //includes array of enemie gameobjects (list didnt give accurate count, needed array)
        //list of gameobject , list of navmehs, list of animators, list of box colliders , list of health , list of initlasied health bools
        //works by adding array if the old value doesnt equal to the new value
        //list works by creating a new list with a value of the array that was created prior
        // then populates the list with the get component feature
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
            ActiveEnemiesAudioSources = new List<AudioSource>(ArrayLength);
            foreach (GameObject Enemies in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                ActiveEnemiesAgents.Add(Enemies.GetComponent<NavMeshAgent>());
                ActiveEnemiesAnimators.Add(Enemies.GetComponent<Animator>());
                ActiveEnemiesBoxColliders.Add(Enemies.GetComponent<BoxCollider>());
                ActiveEnemiesAudioSources.Add(Enemies.GetComponent<AudioSource>());
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
    IEnumerator EnemyDeathCooldown()
    {
        //cooldown for enemy death to stop the mistiming of script coponents 
        yield return new WaitForSecondsRealtime(0.1f);
        EnemyDied = false;
    }
}

