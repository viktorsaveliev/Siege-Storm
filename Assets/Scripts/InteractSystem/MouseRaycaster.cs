using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SiegeStorm.PlayerController
{
    public class MouseRaycaster : MonoBehaviour, IInteractHandler
    {
        public bool IsPointerOverUI { get; private set; }

        public const float MAX_RAY_DISTANCE = 100;

        [SerializeField] private LayerMask _objectLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            IsPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }

        public Vector3 GetTargetPosition()
        {
            if (IsPointerOverUI) return Vector3.zero;

            if (Physics.Raycast(GetRayByMousePosition(), out RaycastHit hitInfo, MAX_RAY_DISTANCE, _groundLayerMask))
            {
                Vector3 targetPosition = hitInfo.point;
                return targetPosition;
            }

            return Vector3.zero;
        }

        private Ray GetRayByMousePosition()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            return _camera.ScreenPointToRay(mousePosition);
        }
    }
}