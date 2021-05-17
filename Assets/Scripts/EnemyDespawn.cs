using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawn : MonoBehaviour
{
    public ScriptsManager SM;
    public int MaxDeadEnemies;
    bool MassRemove;
    [HideInInspector]
    public GameObject[] DeadEnemiesValue;
    [HideInInspector]
    public List<GameObject> DeadEnemies;
    [HideInInspector]
    public List<Animator> DeadEnemiesAnimator;
    //[HideInInspector]
    public List<Collider> DeadEnemiesCollider;
    [HideInInspector]
    public Camera DetectionCam;
   // [HideInInspector]
    public Plane[] planes;
    [HideInInspector]
    public bool DeletingEnemy;
    private SkinnedMeshRenderer[] EnemieSkinnedMeshes;

    void Update()
    {
        if (DetectionCam == null) 
        {
            GameObject TempCam = GameObject.FindGameObjectWithTag("FakeCam");
            DetectionCam = TempCam.GetComponent<Camera>();
        }
        planes = GeometryUtility.CalculateFrustumPlanes(DetectionCam);
        if (DeadEnemiesValue.Length == MaxDeadEnemies) 
        {
            if (MassRemove == false) 
            {
                MassRemove = true;
                MassRemovalOfEnemies();
            }
        }
    }
    private void LateUpdate()
    {
        if (MassRemove == false)
        {
            FetchDeadEnemies();
            NonVisableDetection();
        }
    }
    void MassRemovalOfEnemies() 
    {
        int OldAmmount = (DeadEnemiesValue.Length) / 4;
        for (int i = 0; i < OldAmmount; i++)
        {
            DeadEnemiesCollider[i].enabled = false;
            Destroy(DeadEnemiesValue[i]);
        }
        MassRemove = false;
    }
    void FetchDeadEnemies() 
    {
        DeadEnemiesValue = GameObject.FindGameObjectsWithTag("DeadEnemy");
        DeadEnemies = new List<GameObject>(0);
        DeadEnemiesCollider = new List<Collider>(0);
        DeadEnemiesAnimator = new List<Animator>(0);
        if (DeadEnemies.Count < DeadEnemiesValue.Length)
        {
            int CountValue = 0;
            foreach (GameObject Enemies in GameObject.FindGameObjectsWithTag("DeadEnemy"))
            {
                DeadEnemies.Add(Enemies);
                for (int i = 0; i < SM.LevelScript.HeightOccludedEnemies.Count; i++)
                {
                    if (Enemies == SM.LevelScript.HeightOccludedEnemies[i])
                    {
                        EnemieSkinnedMeshes = Enemies.GetComponentsInChildren<SkinnedMeshRenderer>();
                        foreach (SkinnedMeshRenderer SkinRenders in EnemieSkinnedMeshes)
                        {
                            SkinRenders.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        }
                        EnemieSkinnedMeshes = null;
                        SM.LevelScript.HeightOccludedEnemies.RemoveAt(i);
                    }
                }
                DeadEnemiesCollider.Add(Enemies.GetComponentInChildren<Collider>());
                DeadEnemiesAnimator.Add(Enemies.GetComponent<Animator>());
                if (DeadEnemiesAnimator[CountValue].GetBool("Attack") == true)
                {
                    DeadEnemiesAnimator[CountValue].SetBool("Attack", false);
                    SM.PlayerScript.PlayerAnimator.SetBool("Hurt", false);
                }
                CountValue = CountValue + 1;
            }
        }
    }
    void NonVisableDetection() 
    {
        int ArrayLength = 0;
        foreach (var GameObject in DeadEnemiesValue)
        {
            if (!GeometryUtility.TestPlanesAABB(planes, DeadEnemiesCollider[ArrayLength].bounds) && DeletingEnemy == false)
            {
                DeadEnemiesCollider[ArrayLength].enabled = false;
                Destroy(DeadEnemiesValue[ArrayLength]);
            }
            ArrayLength = ArrayLength + 1;
        } 
    }
}
