using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayer: MonoBehaviour
{
    public int id;
    public string username;
    public Animator PlayerAnimator;
    public CharacterController controller;
    public float moveSpeed = 0.1f;
    public float CurrentMaxHealth;
    public float Health;
    public int Lives = 3;
    public bool PlayerDown;
    private bool[] inputs;
    private bool IsMouseDown;
    public int WeaponValue = 1;
    public bool Firing;
    public bool CanShoot;
    public float AssaultRifleFiringTime = 0.2f;
    public float MiniGunFiringTime = 0.15f;
    public float FlameThrowerFiringTime = 0.15f;
    public float ShotGunFiringTime = 0.75f;
    public float SideArmFiringTime = 0.15f;
    public ServerRaycast ARRayCast;
    public ServerRaycast MGRayCast;
    public ServerRaycast FTRayCast;
    public ServerRaycast[] SGRayCast;
    public ServerRaycast SARayCast;
    public int ARDamage = 10;
    public int MGDamage = 25;
    public int FTDamage = 50;
    public int SGDamage = 50;
    public int SADamage = 5;
    public Ray RayCastRay;
    public RaycastHit NewRayCastHit;
    public GameObject HitTarget;
    public GameObject HitEnemy;
    public bool CanRevive;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        ServerHostingManager.Instance.ConnectedClientsClass.Add(this);
        ServerHostingManager.Instance.ConnectedClients = ServerHostingManager.Instance.ConnectedClients + 1;
        inputs = new bool[4];
    }
    public void FixedUpdate()
    {
        if (!PlayerDown)
        {
            Vector2 _inputDirection = Vector2.zero;
            if (inputs[0])
            {
                _inputDirection.y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.y = -1;
            }
            if (inputs[2])
            {
                _inputDirection.x = -1;
            }
            if (inputs[3])
            {
                _inputDirection.x += 1;
            }
            if (PlayerDown)
            {
                if (inputs[4])
                {
                    Debug.Log("Player Revive");
                }
            }
            Move(_inputDirection);
            if (IsMouseDown)
            {
                StartCoroutine(ShootingMethod());
            }

            if (Firing && !CanShoot)
            {
                RayCastMethod();
                CanShoot = true;
            }
        }
        else
        {
            if (Lives > 0)
            {
                if (CanRevive == true)
                {
                    if (inputs[4])
                    {
                        Health = CurrentMaxHealth;
                        ServerEnemyManager._serverEnemyManager.IgnorePlayer = false;
                        Lives = Lives - 1;
                        PlayerDown = false;
                        ServerSend.SendPlayerRevive(id);
                        Debug.Log("Player Revive");
                    }
                }
            }
        }
    }
    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = new Vector3 (_inputDirection.x, -1 , _inputDirection.y);
        controller.Move((_moveDirection * moveSpeed)*Time.fixedUnscaledDeltaTime);

        ServerSend.PlayerPosition(this);

    }
    public void HealthCalculations() 
    {
        ServerSend.SendEnemyHitPlayer(id, Health);
        if (Health <= 0) 
        {
            Debug.Log(id + "  playerdown");
            PlayerDown = true;
            StartCoroutine(CanReviveTime());
        }
    }
    IEnumerator CanReviveTime() 
    {
        CanRevive = false;
        yield return new WaitForSecondsRealtime(2f);
        CanRevive = true;
    }
    public void NewWeaponValue(int ClientID,int NewWeaponValue) 
    {
        WeaponValue = NewWeaponValue;
        ServerSend.NewWeaponValue(ClientID,this);
    }

    public void SetInputs(bool[] _inputs)
    {
        inputs = _inputs;
    }
    public void SetRotation(Vector3 Rotation)
    {
        if (!PlayerDown)
        {
            Debug.Log("Rotation");
            this.transform.LookAt(Rotation);
            ServerSend.PlayerRotation(this);
        }
    }
    public void MouseInput(bool MouseInput) 
    {
        if (MouseInput) 
        {
            IsMouseDown = true;
        }
        else 
        {
            IsMouseDown = false;
        }
    }
    IEnumerator ShootingMethod()
    {
        if (Firing == false)
        {
            Firing = true;
            switch (WeaponValue)
            {
                case 1:
                    yield return new WaitForSecondsRealtime(AssaultRifleFiringTime);
                    ARRayCast.Shoot = true;
                    CanShoot = false;
                    break;
                case 2:
                    yield return new WaitForSecondsRealtime(MiniGunFiringTime);
                    MGRayCast.Shoot = true;
                    CanShoot = false;
                    break;
                case 3:
                    yield return new WaitForSecondsRealtime(FlameThrowerFiringTime);
                    FTRayCast.Shoot = true;
                    CanShoot = false;
                    break;
                case 4:
                    yield return new WaitForSecondsRealtime(ShotGunFiringTime);
                    foreach (ServerRaycast SGRAY in SGRayCast)
                    {
                        SGRAY.Shoot = true;
                    }
                    CanShoot = false;
                    break;
                case 5:
                    yield return new WaitForSecondsRealtime(SideArmFiringTime);
                    SARayCast.Shoot = true;
                    CanShoot = false;
                    break;
            }
            Firing = false;
            ServerSend.PlayerShoot(this);
        }

    }
    public GameObject FindGameObject(GameObject PerentObject, string Tag)
    {
        Transform[] ChildrenTrasform = PerentObject.GetComponentsInChildren<Transform>(); ;
        Transform FoundTransform = null;
        foreach (Transform ChildTransform in ChildrenTrasform)
        {
            if (ChildTransform.tag == Tag)
            {
                FoundTransform = ChildTransform;
            }
        }
        return (FoundTransform.gameObject);
    }
    public GameObject[] FindMultipleGameObject(GameObject PerentObject, string Tag)
    {
        Transform[] ChildrenTrasform = PerentObject.GetComponentsInChildren<Transform>();
        List<Transform> FoundTransformList = new List<Transform>();
        GameObject[] FoundTransformsArray;
        foreach (Transform ChildTransform in ChildrenTrasform)
        {
            if (ChildTransform.tag == Tag)
            {
                FoundTransformList.Add(ChildTransform);
            }
        }
        FoundTransformsArray = new GameObject[FoundTransformList.Count];

        for (int i = 0; i < FoundTransformsArray.Length; i++)
        {
            FoundTransformsArray[i] = FoundTransformList[i].gameObject;
        }

        return FoundTransformsArray;
    }
    public void RayCastMethod()
    {
        //swtich statment depending on the weapon value to make sure the raycast is shot from the correct point
        //if the raycast hits and enemy it calls the healthmanger method on the enemy script and sets enemy hitted to the raycast collison object in enemy script
        switch (WeaponValue)
        {
            case 1:
                //Rifle RayCast
                GameObject ARNewHit =ARRayCast.HitGameObject;
                if (ARNewHit != null)
                {
                    Debug.Log("pew pew  ");
                    if (ARNewHit.CompareTag("Enemy"))
                    {
                        Debug.Log("Firing hit ememy ");
                        HitEnemy = ARNewHit;
                        ServerEnemyManager._serverEnemyManager.EnemyHited = HitEnemy;
                        if (ServerEnemyManager._serverEnemyManager.HealthCalculation == false)
                        {
                            Debug.Log("Firing Enemy Hit + health caculated = false");
                            ServerPoints._serverPoints.PointsIncrease(id - 1, 10);
                            ServerEnemyManager._serverEnemyManager.CurrentDamage = ARDamage; ;
                            ServerEnemyManager._serverEnemyManager.TakingDamage = true;
                        }
                    }
                    else
                    {
                        if (HitEnemy != null)
                        {
                            HitEnemy = null;
                        }
                    }
                }
                ARRayCast.Shoot = false;
                break;
            //MiniGun Raycast
            case 2:
                GameObject MGNewHit = MGRayCast.HitGameObject;
                if (MGNewHit != null)
                {
                    Debug.Log("pew pew  ");
                    if (MGNewHit.CompareTag("Enemy"))
                    {
                        Debug.Log("Firing hit ememy ");
                        HitEnemy = MGNewHit;
                        ServerEnemyManager._serverEnemyManager.EnemyHited = HitEnemy;
                        if (ServerEnemyManager._serverEnemyManager.HealthCalculation == false)
                        {
                            ServerPoints._serverPoints.PointsIncrease(id - 1, 10);
                            ServerEnemyManager._serverEnemyManager.CurrentDamage = MGDamage;
                            ServerEnemyManager._serverEnemyManager.TakingDamage = true;
                        }
                    }
                    else
                    {
                        if (HitEnemy != null)
                        {
                            HitEnemy = null;
                        }
                    }
                }
                MGRayCast.Shoot = false;
                break;
            //FlameThrower raycast
            case 3:
                GameObject FTNewHit = FTRayCast.HitGameObject;
                if (FTNewHit != null)
                {
                    Debug.Log("pew pew  ");
                    if (FTNewHit.CompareTag("Enemy"))
                    {
                        Debug.Log("Firing hit ememy ");
                        HitEnemy = FTNewHit;
                        ServerEnemyManager._serverEnemyManager.EnemyHited = HitEnemy;
                        if (ServerEnemyManager._serverEnemyManager.HealthCalculation == false)
                        {
                            ServerPoints._serverPoints.PointsIncrease(id - 1, 10);
                            ServerEnemyManager._serverEnemyManager.CurrentDamage = FTDamage;
                            ServerEnemyManager._serverEnemyManager.TakingDamage = true;
                        }
                    }
                    else
                    {
                        if (HitEnemy != null)
                        {
                            HitEnemy = null;
                        }
                    }
                }
                FTRayCast.Shoot = false;
                break;
            //Shotgun Raycast
            case 4:
                for (int i = 0; i < SGRayCast.Length; i++)
                {
                    GameObject SGNewHit = SGRayCast[i].HitGameObject;
                    if (SGNewHit != null)
                    {
                        if (SGNewHit.CompareTag("Enemy"))
                        {
                            HitEnemy = SGNewHit;
                            ServerEnemyManager._serverEnemyManager.EnemyHited = HitEnemy;
                            if (ServerEnemyManager._serverEnemyManager.HealthCalculation == false)
                            {
                                ServerPoints._serverPoints.PointsIncrease(id - 1, 10);
                                ServerEnemyManager._serverEnemyManager.CurrentDamage = SGDamage;
                                ServerEnemyManager._serverEnemyManager.TakingDamage = true;
                            }
                        }
                        else
                        {
                            if (HitEnemy != null)
                            {
                                HitEnemy = null;
                            }
                        }
                    }
                    SGRayCast[i].Shoot = false;
                }
                break;
            case 5:
                GameObject SANewHit = SARayCast.HitGameObject;
                if (SANewHit != null)
                {
                    if (SANewHit.CompareTag("Enemy"))
                    {
                        HitEnemy = SANewHit;
                        ServerEnemyManager._serverEnemyManager.EnemyHited = HitEnemy;
                        if (ServerEnemyManager._serverEnemyManager.HealthCalculation == false)
                        {
                            ServerPoints._serverPoints.PointsIncrease(id - 1, 10);
                            ServerEnemyManager._serverEnemyManager.CurrentDamage = SADamage;
                            ServerEnemyManager._serverEnemyManager.TakingDamage = true;
                        }
                    }
                    else
                    {
                        if (HitEnemy != null)
                        {
                            HitEnemy = null;
                        }
                    }
                }
                SARayCast.Shoot = false;
                break;
        }
    }
}
