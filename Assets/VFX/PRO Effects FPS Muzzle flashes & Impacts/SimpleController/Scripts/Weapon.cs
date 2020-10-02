using Knife.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Effects.SimpleController
{
    /// <summary>
    /// Simple weapon behaviour without reloading and recoil.
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Shot emitters array.
        /// </summary>
        [SerializeField] [Tooltip("Shot emitters array")] private ParticleGroupEmitter[] shotEmitters;
        /// <summary>
        /// After fire particle player.
        /// </summary>
        [SerializeField] [Tooltip("After fire particle player")] private ParticleGroupPlayer afterFireSmoke;
        /// <summary>
        /// Size of bullets.
        /// </summary>
        [SerializeField] [Tooltip("Size of bullets")] private float bulletSize = 1f;
        /// <summary>
        /// Damage type.
        /// </summary>
        [SerializeField] [Tooltip("Damage type")] private DamageTypes damageType = DamageTypes.Bullet;
        /// <summary>
        /// Player camera.
        /// </summary>
        [Tooltip("Player camera")] public Camera playerCamera;
        /// <summary>
        /// Raycast shot mask.
        /// </summary>
        [Tooltip("Raycast shot mask")] public LayerMask ShotMask;
        /// <summary>
        /// Damage amount.
        /// </summary>
        [Tooltip("Damage amount")] public float Damage = 10f;

        /// <summary>
        /// Default fov of player camera.
        /// </summary>
        [Tooltip("Default fov of player camera")] public float DefaultFov = 60f;
        /// <summary>
        /// Aiming fov of player camera.
        /// </summary>
        [Tooltip("Aiming fov of player camera")] public float AimFov = 35f;
        /// <summary>
        /// Automatic fire flag.
        /// </summary>
        [Tooltip("Automatic fire flag")] public bool AutomaticFire;
        /// <summary>
        /// Automatic fire rate in bullets/seconds.
        /// </summary>
        [Tooltip("Automatic fire rate in bullets/seconds")] public float AutomaticFireRate = 10;

        protected Animator handsAnimator;

        private bool isShooted = false;
        private bool isFreezed = false;
        bool isAiming = false;

        float currentFov;

        float lastFireTime;
        float fireInterval
        {
            get
            {
                return 1f / AutomaticFireRate;
            }
        }

        /// <summary>
        /// Current fov of camera.
        /// </summary>
        public float CurrentFov
        {
            get
            {
                return currentFov;
            }
        }

        /// <summary>
        /// Bullet size.
        /// </summary>
        public float BulletSize
        {
            get
            {
                return bulletSize;
            }

            set
            {
                bulletSize = value;
            }
        }

        /// <summary>
        /// Damage type.
        /// </summary>
        public DamageTypes DamageType
        {
            get
            {
                return damageType;
            }

            set
            {
                damageType = value;
            }
        }

        void Start()
        {
            handsAnimator = GetComponent<Animator>();
            lastFireTime = -fireInterval;
        }

        private void OnEnable()
        {
            currentFov = playerCamera.fieldOfView;
            OnEnableHook();
        }

        private void OnDisable()
        {
            currentFov = DefaultFov;
            OnDisableHook();
        }

        protected virtual void OnEnableHook()
        {

        }

        protected virtual void OnDisableHook()
        {

        }

        void Update()
        {
            if (!isFreezed)
            {
                if (AutomaticFire)
                {
                    if (Input.GetMouseButton(0) && Time.time > lastFireTime + fireInterval)
                    {
                        //onAttackEvent.Call(new TargetAttackData(1000, playerCamera.transform.forward));
                        Shot();
                        isShooted = true;
                        lastFireTime = Time.time;
                    }
                    if (Input.GetMouseButtonUp(0) && isShooted)
                    {
                        isShooted = false;
                        EndFire();
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) && Time.time > lastFireTime + fireInterval)
                    {
                        Shot();
                        EndFire();
                        lastFireTime = Time.time;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    isAiming = true;
                }
                if (Input.GetMouseButtonUp(1))
                {
                    isAiming = false;
                }
                OnUpdate();
            }
            currentFov = Mathf.Lerp(currentFov, isAiming ? AimFov : DefaultFov, Time.deltaTime * 12f);
        }

        protected virtual void OnUpdate()
        {

        }

        protected virtual void EndFire()
        {
            PlayAfterFireFX();
        }

        protected void PlayAfterFireFX()
        {
            if (afterFireSmoke != null)
                afterFireSmoke.Play();
        }

        protected void PlayFX()
        {
            if (afterFireSmoke != null)
                afterFireSmoke.Stop();
            if (shotEmitters != null)
            {
                foreach (var e in shotEmitters)
                    e.Emit(1);
            }
        }

        protected virtual void Shot()
        {
            PlayFX();
            handsAnimator.Play("Shot", 0, 0);
            Ray r = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(r, out hitInfo, 1000, ShotMask, QueryTriggerInteraction.Ignore))
            {

                var hittable = hitInfo.collider.GetComponent<IHittable>();
                if (hittable != null)
                {
                    DamageData[] damage = new DamageData[1]
                    {
                    new DamageData()
                    {
                        amount = Damage,
                        direction = r.direction,
                        normal = hitInfo.normal,
                        point = hitInfo.point,
                        size = BulletSize,
                        damageType = DamageType
                    }
                    };

                    hittable.TakeDamage(damage);
                }
            }

            DebugShot(r, hitInfo);
        }

        protected void DebugShot(Ray r, RaycastHit hitInfo)
        {
            if (hitInfo.collider != null)
            {
                Debug.DrawLine(r.origin, hitInfo.point, Color.green, 3f);
            }
            else
            {
                Debug.DrawLine(r.origin, r.GetPoint(1000), Color.red, 3f);
            }
        }

        /// <summary>
        /// Freeze weapon control.
        /// </summary>
        /// <param name="value">is freezed</param>
        public void Freeze(bool value)
        {
            isFreezed = value;
        }
    }
}