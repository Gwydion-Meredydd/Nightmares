using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("Game Controllers")]
    public bool Paused;
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

    void Update()
    {
        if (StartGame == true) 
        {
            Vector3 PlayerStartingPosVector3 = PlayerStartSpawn.position;
            Instantiate(PlayerTypes[PlayerTypeValue - 1], PlayerStartingPosVector3, PlayerStartSpawn.rotation);
            PlayerController.Player = GameObject.FindGameObjectWithTag("Player");
            PlayerController.PlayerCc = PlayerController.Player.GetComponent<CharacterController>();
            GameObject TempShootPoint = GameObject.FindGameObjectWithTag("ShootPoint");
            PlayerController.ShootPoint = TempShootPoint.transform;
            GameObject ChildTempForShootPoint = Instantiate(BulletParticle.gameObject, PlayerController.ShootPoint.position, PlayerController.ShootPoint.rotation);
            ChildTempForShootPoint.transform.parent = PlayerController.ShootPoint;
            PlayerController.AssaultRifleParticle = ChildTempForShootPoint.GetComponent<ParticleSystem>();
            camera_Controller.player = PlayerController.Player.transform;
            StartGame = false;
            InGame = true;
        }   
    }
}
