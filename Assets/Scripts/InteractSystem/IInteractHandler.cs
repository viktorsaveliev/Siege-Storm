using System;
using UnityEngine;

namespace SiegeStorm.PlayerController
{
    public interface IInteractHandler
    {
        public bool IsPointerOverUI { get; }

        public Vector3 GetTargetPosition();
    }
}