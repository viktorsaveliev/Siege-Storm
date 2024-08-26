using System;
using UnityEngine;
using SiegeStorm.Destructibility;

namespace SiegeStorm.FeedbackSystem
{
    [Serializable]
    public class CrackSurfaceFeedback : IActionFeedback
    {
        [SerializeField] private CrackSettings _settings;
        [SerializeField] private Transform _transform;

        public void Active()
        {
            Vector3 crackPosition = _transform.position;
            crackPosition.y += 0.3f;

            FeedbackFacade.CrackSystem.ActiveCrack(_settings, crackPosition);
        }
    }
}