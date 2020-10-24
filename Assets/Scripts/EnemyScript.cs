using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform spawnLocationOne, spawnLocationTwo, spawnLocationThree, spawnLocationFour;
    public Transform player;
    public NavMeshAgent enemy;
    public GameObject gameManager;
    private PlayerController player_Controller;
    public int numberEnemies;
    public bool canSpawn;
    public int waveDelay = 20;
    public PlayerController playerScript;
    public Vector3 playerPosition;

    void Update()
    {
        if (canSpawn == true)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                canSpawn = false;
                StartCoroutine(spawning());                
            }
            //playerPosition = player.transform.position;
            //enemy.SetDestination(playerPosition);
        }
        
    }

    IEnumerator spawning()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < numberEnemies; i++)
        {
            int spawnChoice = Random.Range(1, 4);
            switch (spawnChoice)
            {
                case 1:
                    Instantiate(enemy, spawnLocationOne.position, Quaternion.identity);
                    break;

                case 2:
                    Instantiate(enemy, spawnLocationTwo.position, Quaternion.identity);
                    break;

                case 3:
                    Instantiate(enemy, spawnLocationThree.position, Quaternion.identity);
                    break;

                case 4:
                    Instantiate(enemy, spawnLocationFour.position, Quaternion.identity);
                    break;

                default:
                    Instantiate(enemy, spawnLocationFour.position, Quaternion.identity);
                    break;
            }
        }

        canSpawn = true;
    }

    public Vector3 target()
    {
        return player.transform.position;
    }
}
