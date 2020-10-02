using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager Game_Manager;
    [Header("Selected Player")]
    public GameObject Player;
    public CharacterController PlayerCc;
    [Header("Movement Variables")]
    public float MovingSpeed;
    private Vector3 MovementDirectionValue;
    [Header("WeaponVariables")]
    public int WeaponValue;
    public float ShootSpeed;
    public Transform ShootPoint;
    public Transform CasePoint;
    public float AssaultRifleFiringTime;
    private bool Firing;
    public ParticleSystem AssaultRifleParticle;
    public ParticleSystem AssaultRifleMuzzleFlash;
    public ParticleSystem BulletCasing;
    void Update()
    {
        if (Game_Manager.InGame == true)
        {
            if (Game_Manager.Paused == false)
            {
                //Is called when game is unpased and game is currently active

                PlayerInputs();
            }
        }
    }
    public void PlayerInputs()
    {
        //Methods for player based inputs

        MousePlayerRotation();
        CharacterMovement();
        if (Input.GetMouseButton(0)) 
        {
            StartCoroutine(ShootingMethod());
        }
    }
    #region Player Movement
    public void CharacterMovement()
    {
        //Moves the selected Character in a global position using the character controller
       
        MovementDirectionValue = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        PlayerCc.Move((MovementDirectionValue * MovingSpeed)* Time.deltaTime);
    }
    public void MousePlayerRotation()
    {
        //rotates the players body towards the mouses position

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            Player.transform.LookAt(new Vector3(hit.point.x, Player.transform.position.y, hit.point.z));
        }
    }
    #endregion
    IEnumerator ShootingMethod() 
    {
        if (Firing == false)
        {
            Firing = true;
            //Weapon value indicates which weapon is selected 1= auto rifle 2= mini gun 3= flamethrower , 2 & 3 are drops for limated time
            switch (WeaponValue)
            {
                case 1:
                    AssaultRifleParticle.Emit(1);
                    BulletCasing.Emit(1);
                    AssaultRifleMuzzleFlash.Emit(1);
                    yield return new WaitForSeconds(AssaultRifleFiringTime);
                    Firing = false;
                    Debug.Log("Shot");
                    break;
            }
        }
    }
}
