using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("Game Controllers")]
    public bool Paused;
    public bool AutoStart;
    public bool StartGame;
    public bool InGame;

    [Header("Player Controllers")]
    [Range(1, 4)]
    public int PlayerTypeValue;
    public GameObject[] PlayerTypes;
    public Transform PlayerStartSpawn;
    public PlayerController PlayerController;
    public CameraController camera_Controller;
    public ParticleSystem BulletParticle;
    public ParticleSystem ArMuzzleFlash;
    public ParticleSystem ShellParticle;

    void Update()
    {
        if (AutoStart == true) 
        {
            AutoStart = false;
            StartGame = true;
        }
        if (StartGame == true) 
        {
            Vector3 PlayerStartingPosVector3 = PlayerStartSpawn.position;
            Instantiate(PlayerTypes[PlayerTypeValue - 1], PlayerStartingPosVector3, PlayerStartSpawn.rotation);
            PlayerController.Player = GameObject.FindGameObjectWithTag("Player");
            PlayerController.PlayerCc = PlayerController.Player.GetComponent<CharacterController>();

            GameObject TempShootPoint = GameObject.FindGameObjectWithTag("ShootPoint");
            PlayerController.ARShootPoint = TempShootPoint.transform;
            GameObject ChildTempForShootPoint = Instantiate(BulletParticle.gameObject, PlayerController.ARShootPoint.position, PlayerController.ARShootPoint.rotation);
            ChildTempForShootPoint.transform.parent = PlayerController.ARShootPoint;
            PlayerController.AssaultRifleParticle = ChildTempForShootPoint.GetComponent<ParticleSystem>();

            GameObject ChildTempForMuzzleFlash = Instantiate(ArMuzzleFlash.gameObject, PlayerController.ARShootPoint.position, PlayerController.ARShootPoint.rotation);
            ChildTempForMuzzleFlash.transform.parent = PlayerController.ARShootPoint;
            PlayerController.AssaultRifleMuzzleFlash = ChildTempForMuzzleFlash.GetComponent<ParticleSystem>();

            GameObject TempShellPoint = GameObject.FindGameObjectWithTag("ShellPoint");
            PlayerController.CasePoint = TempShellPoint.transform;
            GameObject ChildTempForCasePoint = Instantiate(ShellParticle.gameObject, PlayerController.CasePoint.position, PlayerController.CasePoint.rotation);
            ChildTempForCasePoint.transform.parent = PlayerController.CasePoint;
            PlayerController.BulletCasing = ChildTempForCasePoint.GetComponent<ParticleSystem>();

            camera_Controller.player = PlayerController.Player.transform;
            StartGame = false;
            InGame = true;
        }   
    }
}
