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
            while (IsFlying)
            {
                TimeElapsed += Time.deltaTime;

                if (TimeElapsed < FlightDuration)
                {
                    float t = TimeElapsed / FlightDuration;

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
