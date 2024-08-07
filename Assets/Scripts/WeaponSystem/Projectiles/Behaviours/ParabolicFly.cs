using System;
using System.Collections;
using UnityEngine;

namespace SiegeStorm
{
    [Serializable]
    public class ParabolicFly : FlyBehaviour
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
                    float height = Mathf.Sin(Mathf.PI * t) * (TargetPosition.y - StartPosition.y + Speed * 0.5f);
                    Target.position = new Vector3(currentPosition.x, height, currentPosition.z);
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