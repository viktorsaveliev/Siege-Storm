using SiegeStorm.InputSystem;
using SiegeStorm.PlayerController;
using SiegeStorm.UISystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SiegeStorm.WeaponSystem
{
    public class PlayerWeapon : MonoBehaviour
    {
        public event Action<Weapon> OnWeaponChanged;

        public enum ShootPhase { Started, Performed, Canceled };

        [SerializeField] private Weapon[] _startedWeapons = new Weapon[MAX_WEAPON_SLOTS];
        [SerializeField] private WeaponSlotUI[] _slotsUI = new WeaponSlotUI[MAX_WEAPON_SLOTS];

        private const int MAX_WEAPON_SLOTS = 3;
        private readonly WeaponSlot[] _weaponSlots = new WeaponSlot[MAX_WEAPON_SLOTS];

        private InputData _inputData;
        private IInteractHandler _interactHandler;
        private int _selectedSlot = -1;
        private Coroutine _shootingCoroutine;

        [Inject] private readonly DiContainer _diContainer;

        private void Awake()
        {
            for(int i = 0; i < MAX_WEAPON_SLOTS; i++)
            {
                _startedWeapons[i].Init(_diContainer, _interactHandler);

                _weaponSlots[i] = new();
                _weaponSlots[i].SetWeapon(_startedWeapons[i]);

                _slotsUI[i].Init(_weaponSlots[i]);
            }
        }

        private void Start()
        {
            SwitchSlot(0);
        }

        private void OnEnable()
        {
            _inputData.OnSwitchWeaponSlot += SwitchSlot;
            _inputData.OnShoot += OnPlayerShoot;
        }

        private void OnDisable()
        {
            _inputData.OnSwitchWeaponSlot -= SwitchSlot;
            _inputData.OnShoot -= OnPlayerShoot;
        }

        [Inject]
        public void Construct(InputData inputData, IInteractHandler interactHandler)
        {
            _inputData = inputData;
            _interactHandler = interactHandler;
        }

        private void SwitchSlot(int slotID)
        {
            if (_selectedSlot == slotID) return;

            if (_selectedSlot != -1)
            {
                _weaponSlots[_selectedSlot].OnDeselect();
            }
            
            _selectedSlot = slotID;
            _weaponSlots[slotID].OnSelect();

            OnWeaponChanged?.Invoke(_weaponSlots[slotID].CurrentWeapon);
        }

        private void Shoot(ShootPhase shootStep)
        {
            _weaponSlots[_selectedSlot].CurrentWeapon.TryShoot(shootStep);
        }

        private void OnPlayerShoot(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                Shoot(ShootPhase.Started);
                _shootingCoroutine = StartCoroutine(ShootingRoutine());
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                if (_shootingCoroutine != null)
                {
                    StopCoroutine(_shootingCoroutine);
                    _shootingCoroutine = null;
                }

                Shoot(ShootPhase.Canceled);
            }
        }

        private IEnumerator ShootingRoutine()
        {
            while (true)
            {
                Shoot(ShootPhase.Performed);
                yield return null;
            }
        }
    }
}