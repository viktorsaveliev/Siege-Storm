using SiegeStorm.InputSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace SiegeStorm.PlayerController
{
    public class MouseRaycaster : MonoBehaviour, IInteractHandler
    {
        public bool IsPointerOverUI { get; private set; }

        public event Action<IInteractable> OnPointerEnter;
        public event Action<IInteractable> OnPointerExit;
        public event Action<IInteractable> OnSelectObject;
        public event Action<Vector3> OnClickGround;

        public const float MAX_RAY_DISTANCE = 100;

        [SerializeField] private LayerMask _objectLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;

        private IInteractable _lastObject;
        private Camera _camera;

        private InputData _inputData;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            _inputData.OnClickLmb += OnPlayerClick;
        }

        private void OnDisable()
        {
            _inputData.OnClickLmb -= OnPlayerClick;
        }

        private void FixedUpdate()
        {
            IsPointerOverUI = EventSystem.current.IsPointerOverGameObject();
            if (IsPointerOverUI) return;

            if (Physics.Raycast(GetRayByMousePosition(), out RaycastHit hitInfo, MAX_RAY_DISTANCE, _objectLayerMask))
            {
                if (hitInfo.transform.TryGetComponent(out IInteractable interactable) && interactable.IsInteractable)
                {
                    if (_lastObject != interactable)
                    {
                        ResetData();

                        _lastObject = interactable;
                        OnPointerEnter?.Invoke(interactable);
                    }
                }
            }
            else
            {
                ResetData();
            }
        }

        [Inject]
        public void Construct(InputData inputData)
        {
            _inputData = inputData;
        }

        private void OnPlayerClick(bool performed)
        {
            if (performed || IsPointerOverUI) return;

            if (_lastObject != null)
            {
                OnSelectObject?.Invoke(_lastObject);
            }
            else
            {
                if (Physics.Raycast(GetRayByMousePosition(), out RaycastHit hitInfo, MAX_RAY_DISTANCE, _groundLayerMask))
                {
                    Vector3 targetPosition = hitInfo.point;
                    OnClickGround?.Invoke(targetPosition);
                }
            }
        }

        private void ResetData()
        {
            if (_lastObject != null)
            {
                OnPointerExit?.Invoke(_lastObject);
                _lastObject = null;
            }
        }

        private Ray GetRayByMousePosition()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            return _camera.ScreenPointToRay(mousePosition);
        }
    }
}