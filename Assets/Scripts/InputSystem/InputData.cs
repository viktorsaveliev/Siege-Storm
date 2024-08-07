using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SiegeStorm.InputSystem
{
    public class InputData : IDisposable
    {
        public event Action<bool> OnClickLmb;
        public event Action<bool> OnPressLmb;
        public event Action<bool> OnScrollWheel;
        public event Action<int> OnSwitchWeaponSlot;

        public event Action OnRotated;
        public event Action OnCanceled;

        private PlayerInputActions _input;
        private MonoBehaviour _monoBeh;

        private bool _isDragging;
        private Vector2 _startMousePosition;
        private const float _dragThreshold = 10f;

        public Vector2 Move { get; private set; }
        public Vector2 Scroll { get; private set; }

        public void Init(MonoBehaviour monoBeh)
        {
            _monoBeh = monoBeh;

            _input = new();
            _input.Enable();

            _input.Player.Move.performed += OnMove;
            _input.Player.Move.canceled += OnMove;

            _input.Player.ScrollWheel.performed += OnScroll;
            _input.Player.ScrollWheel.canceled += OnScroll;

            _input.Player.Select.performed += OnAction;
            _input.Player.Select.canceled += OnAction;

            _input.Player.Rotate.performed += OnRotate;
            _input.Player.Cancel.performed += OnCancel;

            _input.Player.Slot1.performed += ctx => SwitchWeaponSlot(0);
            _input.Player.Slot2.performed += ctx => SwitchWeaponSlot(1);
            _input.Player.Slot3.performed += ctx => SwitchWeaponSlot(2);
        }

        public void Dispose()
        {
            _input.Player.Move.performed -= OnMove;
            _input.Player.Move.canceled -= OnMove;

            _input.Player.ScrollWheel.performed -= OnScroll;
            _input.Player.ScrollWheel.canceled -= OnScroll;

            _input.Player.Select.performed -= OnAction;
            _input.Player.Select.canceled -= OnAction;

            _input.Player.Rotate.performed -= OnRotate;
            _input.Player.Cancel.performed -= OnCancel;

            _input.Player.Slot1.performed -= ctx => SwitchWeaponSlot(0);
            _input.Player.Slot2.performed -= ctx => SwitchWeaponSlot(1);
            _input.Player.Slot3.performed -= ctx => SwitchWeaponSlot(2);
        }

        private void SwitchWeaponSlot(int slotID)
        {
            OnSwitchWeaponSlot?.Invoke(slotID);
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 movementInput = context.ReadValue<Vector2>();
                Move = movementInput;
            }
            else
            {
                Move = Vector2.zero;
            }
        }

        private void OnScroll(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 scroll = context.ReadValue<Vector2>();
                Scroll = scroll;
            }
            else
            {
                Scroll = Vector2.zero;
            }

            OnScrollWheel?.Invoke(context.performed);
        }

        private void OnAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isDragging = false;
                _monoBeh.StartCoroutine(Tick(context));
            }
            else if (context.canceled)
            {
                if (_isDragging)
                {
                    OnPressLmb?.Invoke(false);
                }
                else
                {
                    OnClickLmb?.Invoke(false);
                }
            }
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            OnCanceled?.Invoke();
        }

        private void OnRotate(InputAction.CallbackContext context)
        {
            OnRotated?.Invoke();
        }

        private IEnumerator Tick(InputAction.CallbackContext context)
        {
            _startMousePosition = Mouse.current.position.ReadValue();

            while (context.action.ReadValue<float>() > 0.1f)
            {
                if (!_isDragging)
                {
                    Vector2 currentMousePosition = Mouse.current.position.ReadValue();
                    float dragDistance = Vector2.Distance(_startMousePosition, currentMousePosition);

                    if (dragDistance > _dragThreshold)
                    {
                        OnPressLmb?.Invoke(context.performed);
                        _isDragging = true;
                        yield break;
                    }
                }

                yield return null;
            }
        }
    }
}