using SiegeStorm.PlayerController;
using SiegeStorm.PoolSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SiegeStorm.WeaponSystem
{
    public class Bomber : ProjectileWeapon
    {
        private readonly List<Vector3> _targetPositions = new();

        private const int MaxPoints = 10;
        private const float MinDistanceBetweenPoints = 3.0f;
        private const float MoveSpeed = 10f;

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
                StartCoroutine(BombingRoutine());
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
            foreach (Vector3 targetPosition in _targetPositions)
            {
                yield return MoveToTargetPosition(targetPosition);

                Projectile projectile = Projectiles.GetInactiveObject();
                projectile.Launch(StartProjectilePoint.position, targetPosition, Data.ProjectileSpeed);
            }

            _targetPositions.Clear();
            _lineRenderer.positionCount = 0;

            foreach (Transform indicator in Indicators.PoolList)
            {
                indicator.gameObject.SetActive(false);
            }

            base.Shoot(PlayerWeapon.ShootPhase.Canceled);
        }

        private IEnumerator MoveToTargetPosition(Vector3 targetPosition)
        {
            Vector3 startPosition = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetFlatPosition = new Vector3(targetPosition.x, 0, targetPosition.z);

            float distance = Vector3.Distance(startPosition, targetFlatPosition);

            while (distance > 0.5f)
            {
                float step = MoveSpeed * Time.deltaTime / distance;
                Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, step);
                newPosition.y = transform.position.y;  // ”держиваем высоту на одном уровне
                transform.position = newPosition;

                // ѕоворот самолета по оси Y
                Vector3 directionToTarget = targetFlatPosition - new Vector3(transform.position.x, 0, transform.position.z);
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
                }

                // ќграничение поворота самолета только по оси Y
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                // ќпределение угла наклона (ролла) на основе направлени€
                float rollAngle = Mathf.Clamp(Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up), -45f, 45f);
                Quaternion rollRotation = Quaternion.Euler(0, 0, -rollAngle);

                // ѕрименение наклона по оси Z
                transform.rotation = transform.rotation * rollRotation;

                startPosition = new Vector3(transform.position.x, 0, transform.position.z);
                distance = Vector3.Distance(startPosition, targetFlatPosition);

                yield return null;
            }
        }

    }
}