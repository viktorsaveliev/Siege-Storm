using UnityEngine;
using SiegeStorm.PlayerController;
using SiegeStorm.Destructibility;
using Zenject;

namespace SiegeStorm.FeedbackSystem
{
    public class FeedbackFacade : MonoBehaviour
    {
        public static CameraShake CameraShake;
        public static CrackSystem CrackSystem;

        [Inject]
        public void Construct(CameraShake cameraShake, CrackSystem crackSystem)
        {
            CameraShake = cameraShake;
            CrackSystem = crackSystem;
        }
    }
}