using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Effects
{
    public class RadialMover : MonoBehaviour
    {
        [SerializeField] private float radius = 0.1f;
        [SerializeField] private float fullRadiusTime = 0.5f;
        [SerializeField] private float animationDuration = 1f;

        private float elapsedTime = 0;

        private void OnEnable()
        {
            elapsedTime = 0;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;

            float fraction = elapsedTime / fullRadiusTime;
            fraction = Mathf.Clamp01(fraction);

            float currentRadius = radius * fraction;

            float x = Mathf.Cos(Time.time * animationDuration) * currentRadius;
            float y = Mathf.Sin(Time.time * animationDuration) * currentRadius;

            var pos = transform.localPosition;
            pos.x = x;
            pos.y = y;
            transform.localPosition = pos;
        }
    }
}