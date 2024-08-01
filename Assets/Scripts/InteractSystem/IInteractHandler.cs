using System;
using UnityEngine;

namespace SiegeStorm.PlayerController
{
    public interface IInteractHandler
    {
        public bool IsPointerOverUI { get; }

        public event Action<IInteractable> OnPointerEnter;
        public event Action<IInteractable> OnPointerExit;
        public event Action<IInteractable> OnSelectObject;

        public event Action<Vector3> OnClickGround;
    }
}