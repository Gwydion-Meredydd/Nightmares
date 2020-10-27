using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform [] spawnLocation;
    public GameObject [] enemy;
    public EnemyScript enemyScript;
    public Transform player;
    public int numberEnemies = 10;
    public bool canSpawn = true;
    public int waveDelay = 20;
    public int spawnDelay;

    void Update()
    {
        if (canSpawn == true)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 10)
                //Check to see if number of enemies is getting low to see if more need adding
            {
                canSpawn = false;
                //A control bool so that the method cant be started repeatedly
                StartCoroutine(spawning());                
            }
        }        
    }

    IEnumerator spawning()
    {
        for (int i = 0; i < numberEnemies; i++)
            //Loop to spawn the enemies
        {
            int spawnNumber = Random.Range(0, 4);
            //choose which location to spawn the enemies at
            GameObject newEnemy = Instantiate(enemy[i].gameObject, spawnLocation[spawnNumber].transform.position, Quaternion.identity) as GameObject;
            //Spawn the enemies
            yield return new WaitForSeconds(spawnDelay);
        }

        canSpawn = true;
        //make to script capable of running again
    }
}
