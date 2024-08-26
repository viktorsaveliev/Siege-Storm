using UnityEngine;

namespace SiegeStorm.FeedbackSystem
{
    public class CameraShakeFeedback : IActionFeedback
    {
        [SerializeField, Range(0.3f, 3f)] private float _duration = 0.5f;
        [SerializeField, Range(0.1f, 1f)] private float _strength = 0.3f;
        [SerializeField, Range(1, 15)] private int _vibrato = 10;
        [SerializeField, Range(1, 90)] private float _randomness = 90f;

        public void Active()
        {
            FeedbackFacade.CameraShake.Shake(_duration, _strength, _vibrato, _randomness);
        }
    }
}