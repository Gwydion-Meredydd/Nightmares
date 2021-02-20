using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    public GameObject[] Levels;
    public GameObject SpawnedLevel;
    [Space]
    [Range(1, 5)]
    public int LevelValue;
    [Space]
    public Vector3[] NavMeshBuildArea;
    [Space]
    public Vector3[] CameraMinSpace;
    public Vector3[] CameraMaxSpace;
    [Space]
    public float[] YCamValue;
    [Space]
    public float HeightOcclusionValue;
    public float[] HeighOcclusionLevel;
    public GameObject[] HeightOcclusionObjects;
    public GameObject[] HeightOcclusionReplacementObjects;
    public List <GameObject> HeightOccludedEnemies;

    public void SpawnLevel() 
    {
        int LevelArrayValue = LevelValue -1;
        Instantiate(Levels[LevelArrayValue], new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        SM.CameraScript.minPosition = CameraMinSpace[LevelArrayValue];
        SM.CameraScript.maxPosition = CameraMaxSpace[LevelArrayValue];
        SM.NavMeshAreaBuilder.m_Size = NavMeshBuildArea[LevelArrayValue];
        SM.NavMeshAreaBuilder.LevelReadyForNavmesh();
        SM.CameraScript.yValue = YCamValue[LevelArrayValue];
        SM.CameraScript.HoldingYValue = YCamValue[LevelArrayValue];
        HeightOcclusionValue = HeighOcclusionLevel[LevelArrayValue];
        SpawnedLevel = GameObject.FindGameObjectWithTag("Level");
    }
}
