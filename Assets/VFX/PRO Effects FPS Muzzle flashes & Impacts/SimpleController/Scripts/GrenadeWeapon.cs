using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Effects.SimpleController
{
    /// <summary>
    /// GrenadeWeapon component
    /// </summary>
    public class GrenadeWeapon : Weapon
    {
        /// <summary>
        /// Throwing grenade prefab.
        /// </summary>
        [SerializeField] [Tooltip("Throwing grenade prefab")] private GameObject grenadePrefab;
        /// <summary>
        /// Grenade spawn point.
        /// </summary>
        [SerializeField] [Tooltip("Grenade spawn point")] private Transform grenadeSpawnPoint;
        /// <summary>
        /// Player root transform.
        /// </summary>
        [SerializeField] [Tooltip("Player root transform")] private GameObject playerRoot;
        /// <summary>
        /// Hand grenade (disables after throwing).
        /// </summary>
        [SerializeField] [Tooltip("Hand grenade (disables after throwing)")] private GameObject handProp;
        /// <summary>
        /// Enable hand grenade delay in seconds.
        /// </summary>
        [SerializeField] [Tooltip("Enable hand grenade delay in seconds")] private float enableHandPropDelay = 1f;

        private Collider[] playerColliders;

        private float elapsedTime = 0f;
        private bool isHandPropDisabled = false;

        protected override void OnEnableHook()
        {
            playerColliders = playerRoot.GetComponents<Collider>();
            isHandPropDisabled = false;
            handProp.SetActive(true);
        }

        protected override void Shot()
        {
            handsAnimator.SetBool("Hold", true);
        }

        protected override void EndFire()
        {
            handsAnimator.SetBool("Hold", false);
        }

        private void SpawnGrenade()
        {
            var instance = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Quaternion.LookRotation(playerCamera.transform.forward));
            var ignoreCollision = instance.GetComponent<ICollisionIgnore>();
            if (ignoreCollision != null)
            {
                foreach (var c in playerColliders)
                {
                    ignoreCollision.IgnoreCollision(c);
                }
            }

            handProp.SetActive(false);
            elapsedTime = 0;
            isHandPropDisabled = true;
        }

        protected override void OnUpdate()
        {
            if (isHandPropDisabled)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= enableHandPropDelay)
                {
                    handProp.SetActive(true);
                    isHandPropDisabled = false;
                }
            }
        }
    }
}