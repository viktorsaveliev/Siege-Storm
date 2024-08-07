using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SiegeStorm.Destructibility
{
    public class CrackSystem : MonoBehaviour
    {
        [SerializeField, Range(1, 20)] private float _delayForInactiveCrack;
        [SerializeField, Range(1, 10)] private float _fadeDuration;

        [SerializeField] private CrackSettings _lightCracks;
        [SerializeField] private CrackSettings _smallCracks;

        private WaitForSeconds _delay;

        private void Awake()
        {
            _delay = new(_delayForInactiveCrack);

            _lightCracks.Init(transform);
            _smallCracks.Init(transform);
        }

        public void ActiveCrack(CrackSettings crackSettings, Vector3 position)
        {
            DecalProjector crack = crackSettings.GetInactiveCrack();

            crack.transform.position = position;
            crack.gameObject.SetActive(true);

            StartCoroutine(InactiveDelay(crack));
        }

        private IEnumerator InactiveDelay(DecalProjector crack)
        {
            yield return _delay;

            float startFadeFactor = crack.fadeFactor;
            float elapsedTime = 0f;

            while (elapsedTime < _fadeDuration)
            {
                crack.fadeFactor = Mathf.Lerp(startFadeFactor, 0f, elapsedTime / _fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            crack.gameObject.SetActive(false);
            crack.fadeFactor = 1f;
        }
    }
}