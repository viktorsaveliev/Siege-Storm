using DG.Tweening;
using UnityEngine;

namespace SiegeStorm.PlayerController
{
    public class CameraShake : MonoBehaviour
    {
        private Camera _camera;
        private Tween _currentShakeTween;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnDestroy()
        {
            _currentShakeTween?.Kill();
        }

        public void Shake(float duration = 0.5f, float strength = 0.3f, int vibrato = 10, float randomness = 90f)
        {
            _currentShakeTween?.Kill();
            _currentShakeTween = _camera.transform.DOShakeRotation(duration, strength, vibrato, randomness);
        }

        public void StopShake()
        {
            if (_currentShakeTween != null && _currentShakeTween.IsActive())
            {
                _currentShakeTween.Kill();
            }
        }
    }
}
