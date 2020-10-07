using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform spawnLocationOne, spawnLocationTwo, spawnLocationThree, spawnLocationFour;
    private GameObject player;
    public GameObject enemy;
    public int numberEnemies;
    bool canSpawn = true;
    public int waveDelay = 20;

    void Update()
    {
        if (canSpawn == true)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                canSpawn = false;
                StartCoroutine(spawning());                
            }
        }
    }

    IEnumerator spawning()
    {
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
        yield return new WaitForSeconds(5);
        canSpawn = true;
    }
}
