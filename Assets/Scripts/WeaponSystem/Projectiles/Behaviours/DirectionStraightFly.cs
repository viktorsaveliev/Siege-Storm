using System;
using System.Collections;
using UnityEngine;

namespace SiegeStorm
{
    [Serializable]
    public class DirectionStraightFly : FlyBehaviour
    {
        [SerializeField, Range(1, 100)] private float _distance = 50f;

        public override IEnumerator StartFly()
        {
            TargetPosition.y = Target.position.y;

            Vector3 direction = (TargetPosition - StartPosition).normalized;
            Vector3 endPosition = StartPosition + direction * _distance;

            while (IsFlying)
            {
                TimeElapsed += Time.deltaTime;

                if (TimeElapsed < 1)
                {
                    float t = TimeElapsed / 1;

                    Vector3 currentPosition = Vector3.Lerp(StartPosition, endPosition, t);
                    Target.position = currentPosition;
                }
                else
                {
                    IsFlying = false;
                    EndFly();
                }

                yield return null;
            }
        }
    }
}