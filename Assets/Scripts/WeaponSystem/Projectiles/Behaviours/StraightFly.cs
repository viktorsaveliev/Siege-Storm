using System;
using System.Collections;
using UnityEngine;

namespace SiegeStorm
{
    [Serializable]
    public class StraightFly : FlyBehaviour
    {
        public override IEnumerator StartFly()
        {
            FlySpeed = Vector3.Distance(StartPosition, TargetPosition) / FlySpeed;

            while (IsFlying)
            {
                TimeElapsed += Time.deltaTime;

                if (TimeElapsed < FlySpeed)
                {
                    float t = TimeElapsed / FlySpeed;

                    Vector3 currentPosition = Vector3.Lerp(StartPosition, TargetPosition, t);
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
