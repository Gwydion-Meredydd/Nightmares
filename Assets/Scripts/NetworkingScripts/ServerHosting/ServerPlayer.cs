using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayer: MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public float moveSpeed = 0.1f;
    private bool[] inputs;
    private bool IsMouseDown;
    public int WeaponValue = 1;
    public bool Firing;
    public bool CanShoot;
    private float AssaultRifleFiringTime = 0.2f;
    private float MiniGunFiringTime = 0.15f;
    private float FlameThrowerFiringTime = 0.15f;
    private float ShotGunFiringTime = 0.75f;
    private float SideArmFiringTime = 0.15f;
    private GameObject AR;
    public Transform ARShootPoint;
    private GameObject MG;
    public Transform MGShootPoint;
    private GameObject FT;
    public Transform FTShootPoint;
    private GameObject SG;
    public Transform[] SGShootPoint;
    private GameObject SA;
    public Transform SAShootPoint;
    public Ray RayCastRay;
    public RaycastHit RayCastHit;
    public GameObject HitTarget;
    public GameObject HitEnemy;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        ServerHostingManager.Instance.ConnectedClientsClass.Add(this);
        ServerHostingManager.Instance.ConnectedClients = ServerHostingManager.Instance.ConnectedClients + 1;
        inputs = new bool[4];
        AR = FindGameObject(this.gameObject, "AR");
        MG = FindGameObject(this.gameObject, "MG");
        FT = FindGameObject(this.gameObject, "FT");
        SG = FindGameObject(this.gameObject, "SG");
        SA = FindGameObject(this.gameObject, "SA");
        ARShootPoint = FindGameObject(AR, "ARShootPoint").transform;
        MGShootPoint = FindGameObject(MG, "MGShootPoint").transform;
        FTShootPoint = FindGameObject(MG, "FTShootPoint").transform;
        GameObject[] ClientShotgunTempShootPoint = FindMultipleGameObject(SG, "SGShootPoint");
        SGShootPoint = new Transform[ClientShotgunTempShootPoint.Length];
        for (int i = 0; i < ClientShotgunTempShootPoint.Length; i++)
        {
            Debug.Log(i);
            SGShootPoint[i] = ClientShotgunTempShootPoint[i].transform;
        }
        SAShootPoint = FindGameObject(MG, "SAShootPoint").transform;
    }
    public void FixedUpdate()
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

    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = new Vector3 (_inputDirection.x, -1 , _inputDirection.y);
        controller.Move((_moveDirection * moveSpeed)*Time.fixedUnscaledDeltaTime);

        ServerSend.PlayerPosition(this);

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
        this.transform.LookAt(Rotation);
        ServerSend.PlayerRotation(this);
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
                    CanShoot = false;
                    break;
                case 2:
                    yield return new WaitForSecondsRealtime(MiniGunFiringTime);
                    CanShoot = false;
                    break;
                case 3:
                    yield return new WaitForSecondsRealtime(FlameThrowerFiringTime);
                    CanShoot = false;
                    break;
                case 4:
                    yield return new WaitForSecondsRealtime(ShotGunFiringTime);
                    CanShoot = false;
                    break;
                case 5:
                    yield return new WaitForSecondsRealtime(SideArmFiringTime);
                    CanShoot = false;
                    break;
            }
            ServerSend.PlayerShoot(this);
            Firing = false;
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
                if (Physics.Raycast(ARShootPoint.position, ARShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        //enemy hit goes here
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
            //MiniGun Raycast
            case 2:
                if (Physics.Raycast(MGShootPoint.position, MGShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        //enemy hit goes here
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
            //FlameThrower raycast
            case 3:
                if (Physics.Raycast(FTShootPoint.position, FTShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                        //enemy hit goes here
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
            //Shotgun Raycast
            case 4:
                for (int i = 0; i < SGShootPoint.Length; i++)
                {
                    if (Physics.Raycast(SGShootPoint[i].position, SGShootPoint[i].transform.forward, out RayCastHit))
                    {
                        if (RayCastHit.transform.tag == "Enemy")
                        {
                            //enemy hit goes here
                        }
                        else
                        {
                            HitEnemy = null;
                        }
                    }
                }
                break;
            case 5:
                if (Physics.Raycast(SAShootPoint.position, SAShootPoint.transform.forward, out RayCastHit))
                {
                    if (RayCastHit.transform.tag == "Enemy")
                    {
                       //enemy hit goes here
                    }
                    else
                    {
                        HitEnemy = null;
                    }
                }
                break;
        }
    }
}
