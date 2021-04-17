using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientEnemyManager : MonoBehaviour
{
    public GameObject[] GroundEnemies;
    public GameObject[] NormalEnemies;
    public List<GameObject> Enemies;
    public List<Animator> EnemieAnimators;
    public List<AudioSource> EnemieAudioSources;
    public void SpawnGroundEnemy(Quaternion Rotation, Vector3 Position , int RandomValue) 
    {
        GameObject NewEnemie = Instantiate(GroundEnemies[RandomValue], Position, Rotation);
        EnemieSpawned(NewEnemie);
    }
    public void SpawnNormalEnemy(Quaternion Rotation, Vector3 Position , int RandomValue) 
    {
        GameObject NewEnemie =  Instantiate(NormalEnemies[RandomValue], Position, Rotation);
        EnemieSpawned(NewEnemie);
    }
    public void EnemieSpawned(GameObject SpawnedEnemie) 
    {
        Enemies.Add(SpawnedEnemie);
        Animator TempAnimator = SpawnedEnemie.GetComponent<Animator>();
        EnemieAnimators.Add(TempAnimator);
        AudioSource TempAudioSource = SpawnedEnemie.GetComponent<AudioSource>();
        EnemieAudioSources.Add(TempAudioSource);
    }
    public void UpdateEnemyTransforms(Quaternion[] NewRotation, Vector3[] NewPosition) 
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].transform.position = Vector3.Lerp(Enemies[i].transform.position, NewPosition[i], Time.fixedDeltaTime * 10);
            Enemies[i].transform.rotation = Quaternion.Lerp(Enemies[i].transform.rotation, NewRotation[i], Time.fixedDeltaTime * 10);
        }
        
    }
}
