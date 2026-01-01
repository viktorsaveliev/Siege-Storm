using SiegeStorm.PlayerController;
using SiegeStorm.PoolSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Zenject;

namespace SiegeStorm.WeaponSystem
{
    public class Bomber : ProjectileWeapon
    {
        public event Action OnReachedPoint;

        private readonly List<Vector3> _targetPositions = new();

        private const int MaxPoints = 10;
        private const float MinDistanceBetweenPoints = 3.0f;
        private const float MoveSpeed = 10f;

        [SerializeField] private GameObject _airplaneModel;
        [SerializeField] private Transform[] _startPoints;
        [SerializeField] private Transform _indicatorPrefab;
        [SerializeField] private LineRenderer _lineRenderer;

        private ObjectPool<Transform> Indicators;

        public override void Init(DiContainer diContainer, IInteractHandler interactHandler)
        {
            base.Init(diContainer, interactHandler);
            _lineRenderer.positionCount = 0;

            Indicators = new(_indicatorPrefab, null, MaxPoints);
            Indicators.CreatePool();
        }

        protected override void Shoot(PlayerWeapon.ShootPhase shootStep)
        {
            if (shootStep == PlayerWeapon.ShootPhase.Performed)
            {
                if (_targetPositions.Count == 0)
                {
                    AddFirstTargetPosition();
                }
                else if (_targetPositions.Count < MaxPoints)
                {
                    Vector3 lastPosition = _targetPositions[^1];
                    Vector3 currentTargetPosition = GetTargetPosition();

                    if (Vector3.Distance(lastPosition, currentTargetPosition) >= MinDistanceBetweenPoints)
                    {
                        AddTargetPosition();
                    }
                }
            }
            else if (shootStep == PlayerWeapon.ShootPhase.Canceled)
            {
                if (_targetPositions.Count == 0)
                {
                    Debug.LogError("Target Positions Count = 0");
                    return;
                }

                Transform point = GetClosestPoint(_targetPositions[0]);
                transform.position = new(point.position.x, transform.position.y, point.position.z);
                _airplaneModel.SetActive(true);

                StartCoroutine(BombingRoutine());
            }
        }

        protected override void OnShoot()
        {
            base.OnShoot();

            MoveToClosestPoint();
            OnReachedPoint += HideWeapon;

            void HideWeapon()
            {
                OnReachedPoint -= HideWeapon;
                _airplaneModel.SetActive(false);
            }
        }

        private void AddFirstTargetPosition()
        {
            _targetPositions.Clear();
            _lineRenderer.positionCount = 0;
            AddTargetPosition();
        }

        private void AddTargetPosition()
        {
            Vector3 targetPosition = GetTargetPosition();
            if (targetPosition != Vector3.zero)
            {
                _targetPositions.Add(targetPosition);

                Transform indicator = Indicators.GetInactiveObject();
                indicator.position = targetPosition;
                indicator.gameObject.SetActive(true);

                _lineRenderer.positionCount = _targetPositions.Count;
                _lineRenderer.SetPositions(_targetPositions.ToArray());
            }
        }

        private IEnumerator BombingRoutine()
        {
            foreach (Transform indicator in Indicators.PoolList)
            {
                indicator.gameObject.SetActive(false);
            }

            foreach (Vector3 targetPosition in _targetPositions)
            {
                yield return MoveToTargetPosition(targetPosition);

                Projectile projectile = Projectiles.GetInactiveObject();
                projectile.Launch(StartProjectilePoint.position, targetPosition, Data.ProjectileSpeed);
            }

            _targetPositions.Clear();
            _lineRenderer.positionCount = 0;

            base.Shoot(PlayerWeapon.ShootPhase.Canceled);
        }

        private IEnumerator MoveToTargetPosition(Vector3 targetPosition)
        {
            Vector3 startPosition = new(transform.position.x, 0, transform.position.z);
            Vector3 targetFlatPosition = new(targetPosition.x, 0, targetPosition.z);

            float distance = Vector3.Distance(startPosition, targetFlatPosition);

            while (distance > 0.5f)
            {
                float step = MoveSpeed * Time.deltaTime / distance;
                Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, step);
                newPosition.y = transform.position.y;
                transform.position = newPosition;

                Vector3 directionToTarget = targetFlatPosition - new Vector3(transform.position.x, 0, transform.position.z);
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
                }

                startPosition = new Vector3(transform.position.x, 0, transform.position.z);
                distance = Vector3.Distance(startPosition, targetFlatPosition);

                yield return null;
            }

            OnReachedPoint?.Invoke();
        }

        private void MoveToClosestPoint()
        {
            Transform closestPoint = GetClosestPoint(transform.position);
            Vector3 targetPosition = new(closestPoint.position.x, transform.position.y, closestPoint.position.z);
            StartCoroutine(MoveToTargetPosition(targetPosition));
        }

        private Transform GetClosestPoint(Vector3 position)
        {
            Transform closestPoint = null;
            float closestDistance = float.MaxValue;

            foreach(Transform point in _startPoints)
            {
                float distance = Vector3.Distance(position, point.position);

                if (distance < closestDistance)
                {
                    closestPoint = point;
                    closestDistance = distance;
                }
            }

            return closestPoint;
        }
    }
}