using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientEnemyManager : MonoBehaviour
{
    public GameObject[] GroundEnemies;
    public GameObject[] NormalEnemies;
    public List<GameObject> Enemies;
    public List<int> EnemiesHealth;

    public List<Animator> EnemieAnimators;
    public List<AudioSource> EnemieAudioSources;
    public bool TakingDamge;
    public void SpawnGroundEnemy(Quaternion Rotation, Vector3 Position, int RandomValue)
    {
        GameObject NewEnemie = Instantiate(GroundEnemies[RandomValue], Position, Rotation);
        EnemieSpawned(NewEnemie);
    }
    public void SpawnNormalEnemy(Quaternion Rotation, Vector3 Position, int RandomValue)
    {
        GameObject NewEnemie = Instantiate(NormalEnemies[RandomValue], Position, Rotation);
        EnemieSpawned(NewEnemie);
    }
    public void EnemieSpawned(GameObject SpawnedEnemie)
    {
        Enemies.Add(SpawnedEnemie);
        EnemiesHealth.Add(100);
        Animator TempAnimator = SpawnedEnemie.GetComponent<Animator>();
        EnemieAnimators.Add(TempAnimator);
        AudioSource TempAudioSource = SpawnedEnemie.GetComponent<AudioSource>();
        EnemieAudioSources.Add(TempAudioSource);
    }
    public void UpdateEnemyTransforms(Quaternion[] NewRotation, Vector3[] NewPosition)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {

            if (Enemies[i] != null)
            {
                if (i <= Enemies.Count)
                {
                    if (!TakingDamge)
                    {
                        Enemies[i].transform.position = Vector3.Lerp(Enemies[i].transform.position, NewPosition[i], Time.fixedDeltaTime * 10);
                        Enemies[i].transform.rotation = Quaternion.Lerp(Enemies[i].transform.rotation, NewRotation[i], Time.fixedDeltaTime * 10);
                    }
                }
            }

        }
    }
    public void EnemyDamaged(int EnemyArrayValue, int NewHealth)
    {
        TakingDamge = true;
        EnemiesHealth[EnemyArrayValue] = NewHealth;
        #region Color Damage
        Color newcolor = new Color(1, 1, 1, 1);
        if (EnemiesHealth[EnemyArrayValue] < 10)
        {
            newcolor = new Color(1, 0.1f, 0.1f, 1);
        }
        else if (EnemiesHealth[EnemyArrayValue] < 20)
        {
            newcolor = new Color(1, 0.2f, 0.2f, 1);
        }
        else if (EnemiesHealth[EnemyArrayValue] < 30)
        {
            newcolor = new Color(1, 0.3f, 0.3f, 1);
        }
        else if (EnemiesHealth[EnemyArrayValue] < 40)
        {
            newcolor = new Color(1, 0.4f, 0.4f, 1);
        }
        else if (EnemiesHealth[EnemyArrayValue] < 50)
        {
            newcolor = new Color(1, 0.5f, 0.5f, 1);
        }
        else if (EnemiesHealth[EnemyArrayValue] < 60)
        {
            newcolor = new Color(1, 0.6f, 0.6f, 1);
        }
        else if (EnemiesHealth[EnemyArrayValue] < 70)
        {
            newcolor = new Color(1, 0.7f, 0.7f, 1);
        }
        else if (EnemiesHealth[EnemyArrayValue] < 80)
        {
            newcolor = new Color(1, 0.8f, 0.8f, 1);
        }
        else if (EnemiesHealth[EnemyArrayValue] < 90)
        {
            newcolor = new Color(1, 0.9f, 0.9f, 1);
        }
        Renderer[] renderers = Enemies[EnemyArrayValue].GetComponentsInChildren<Renderer>();
        foreach (var render in renderers)
        {
            render.material.SetColor("_Color", newcolor);
        }
        #endregion
        if (EnemiesHealth[EnemyArrayValue] <= 0)
        {

            EnemieAnimators[EnemyArrayValue].SetBool("Hit", false);
            EnemieAnimators[EnemyArrayValue].SetBool("Dead", true);
            EnemieAnimators[EnemyArrayValue].SetBool("Attack", false);

            int DeathValue = Random.Range(1, 4);
            EnemieAnimators[EnemyArrayValue].SetInteger("DeathRandomiser", DeathValue);

            Enemies[EnemyArrayValue].tag = "DeadEnemy";
            Enemies.RemoveAt(EnemyArrayValue);
            EnemiesHealth.RemoveAt(EnemyArrayValue);
            EnemieAudioSources.RemoveAt(EnemyArrayValue);
            EnemieAnimators.RemoveAt(EnemyArrayValue);
        }
        TakingDamge = false;
    }
    public void EnemyAttackRecevied(int arrayvalue)
    {
        StartCoroutine(AttackingTime(arrayvalue));
    }
    IEnumerator AttackingTime(int ArrayLength)
    {
        int AttackValue = Random.Range(1, 3);
        EnemieAnimators[ArrayLength].SetInteger("AttackRandomiser", AttackValue);
        EnemieAnimators[ArrayLength].SetBool("Moving", false);
        EnemieAnimators[ArrayLength].SetBool("Attack", true);
        //ActiveEnemiesAudioSources[ArrayLength].PlayOneShot(SM.AudioScripts.EnemyAttacking[(Random.Range(0, 3))]);

        yield return new WaitForSecondsRealtime(0.1f);
        if (ArrayLength <= EnemieAnimators.Count)
        {
            if (EnemieAnimators[ArrayLength] != null)
            {
                EnemieAnimators[ArrayLength].SetBool("Moving", true);
                EnemieAnimators[ArrayLength].SetBool("Attack", false);
            }
        }
    }
}
