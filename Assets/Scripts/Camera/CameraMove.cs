using SiegeStorm.InputSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SiegeStorm.PlayerController
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private MouseRaycaster _mouseRaycaster;

        [Header("Speeds")]
        [SerializeField, Range(0.5f, 5f)] private float _moveSpeed = 1f;
        [SerializeField, Range(0.2f, 3f)] private float _zoomSpeed = 1f;
        [SerializeField, Range(0.1f, 1f)] private float _smoothTime = 0.2f;

        [Header("Limits")]
        [SerializeField] private Vector2 _minLimitMove;
        [SerializeField] private Vector2 _maxLimitMove;

        private IInteractHandler _objectFinder;
        private InputData _inputData;

        private bool _isDragging;
        private Vector2 _mouseDelta;

        private const float _minScrollY = 5f;
        private const float _maxScrollY = 15f;

        private Vector3 _velocity = Vector3.zero;

        private Vector3 _targetPosition;
        private Coroutine _coroutine;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _targetPosition = _camera.transform.position;

            _objectFinder = _mouseRaycaster.GetComponent<IInteractHandler>();
        }

        private void OnEnable()
        {
            _inputData.OnPressLmb += OnMoveCamera;
            _inputData.OnScrollWheel += OnScroll;
        }

        private void OnDisable()
        {
            _inputData.OnPressLmb -= OnMoveCamera;
            _inputData.OnScrollWheel -= OnScroll;
        }

        private void OnScroll(bool performed)
        {
            if (_objectFinder.IsPointerOverUI) return;

            float zoomAmount = _inputData.Scroll.y * _zoomSpeed * Time.deltaTime;
            Vector3 verticalMovement = Vector3.up * zoomAmount;
            _targetPosition -= verticalMovement;
            _targetPosition.y = Mathf.Clamp(_targetPosition.y, _minScrollY, _maxScrollY);

            StartTick();
        }

        private void OnMoveCamera(bool performed)
        {
            if (_objectFinder.IsPointerOverUI) return;

            _isDragging = performed;
            if (_isDragging)
            {
                StartTick();
            }
        }

        private void StartTick()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            while (_isDragging || _camera.transform.position != _targetPosition)
            {
                if (_isDragging)
                {
                    _mouseDelta = Mouse.current.delta.ReadValue();
                    _targetPosition += _moveSpeed * Time.deltaTime * new Vector3(-_mouseDelta.y, 0, _mouseDelta.x);
                    _targetPosition.x = Mathf.Clamp(_targetPosition.x, _minLimitMove.x, _maxLimitMove.x);
                    _targetPosition.z = Mathf.Clamp(_targetPosition.z, _minLimitMove.y, _maxLimitMove.y);
                }

                if (_camera.transform.position != _targetPosition)
                {
                    _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, _targetPosition, ref _velocity, _smoothTime);
                }

                yield return null;
            }

            _coroutine = null;
        }

        [Inject]
        public void Construct(InputData inputData)
        {
            _inputData = inputData;
        }
    }
}