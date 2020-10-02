using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Effects.SimpleController
{
    /// <summary>
    /// Flashbang grenade component.
    /// </summary>
    public class FlashbangGrenade : MonoBehaviour, ICollisionIgnore
    {
        /// <summary>
        /// Explode activation timer in seconds.
        /// </summary>
        [SerializeField] [Tooltip("Explode activation timer in seconds")] private float activateTimer = 3f;
        /// <summary>
        /// Explode particles that will be enabled.
        /// </summary>
        [SerializeField] [Tooltip("Explode particles that will be enabled")] private ParticleSystem[] particles;
        /// <summary>
        /// Initial velocity of grenade (direction is forward of tranform).
        /// </summary>
        [SerializeField] [Tooltip("Start velocity of grenade (direction is forward of tranform)")] private float speed = 5f;
        /// <summary>
        /// Initial angular speed of grenade.
        /// </summary>
        [SerializeField] [Tooltip("Start angular speed of grenade")] private Vector3 angularSpeed = new Vector3(60, 0, 0);
        /// <summary>
        /// Explosion radius.
        /// </summary>
        [SerializeField] [Tooltip("Explosion radius")] private float radius = 20f;

        private Rigidbody body;
        private Collider grenadeCollider;
        private float elapsedTime = 0;

        private bool isActivated = false;
        private bool isEnded = false;

        private void OnEnable()
        {
            body = GetComponent<Rigidbody>();
            body.velocity = transform.forward * speed;
            body.angularVelocity = angularSpeed;
            grenadeCollider = GetComponent<Collider>();

            foreach (var p in particles)
            {
                p.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// ICollisionIgnore.IgnoreCollision implementation. Enable ignoring of collision between grenade collider and collider.
        /// </summary>
        /// <param name="collider">collider that should be ignored</param>
        public void IgnoreCollision(Collider collider)
        {
            Physics.IgnoreCollision(collider, grenadeCollider);
        }

        private void Update()
        {
            if (isEnded)
                return;

            if (!isActivated)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= activateTimer)
                {
                    Activate();
                }
            }
        }

        private void Activate()
        {
            isActivated = true;
            elapsedTime = 0;

            foreach (var p in particles)
            {
                p.gameObject.SetActive(true);
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach(var c in colliders)
            {
                var blinder = c.GetComponentInChildren<IBlinder>();

                if(blinder != null)
                {
                    blinder.Blind(1f, transform.position);
                }
            }
        }
    }
}