using SiegeStorm.InputSystem;
using SiegeStorm.PlayerController;
using SiegeStorm.UISystem;
using System;
using UnityEngine;
using Zenject;

namespace SiegeStorm.WeaponSystem
{
    public class PlayerWeapon : MonoBehaviour
    {
        public event Action<Weapon> OnWeaponChanged;

        [SerializeField] private Weapon[] _startedWeapons = new Weapon[MAX_WEAPON_SLOTS];
        [SerializeField] private WeaponSlotUI[] _slotsUI = new WeaponSlotUI[MAX_WEAPON_SLOTS];

        private const int MAX_WEAPON_SLOTS = 3;
        private readonly WeaponSlot[] _weaponSlots = new WeaponSlot[MAX_WEAPON_SLOTS];

        private InputData _inputData;
        private IInteractHandler _interactHandler;
        private int _selectedSlot = -1;

        [Inject] private readonly DiContainer _diContainer;

        private void Awake()
        {
            for(int i = 0; i < MAX_WEAPON_SLOTS; i++)
            {
                _startedWeapons[i].Init(_diContainer);

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
            _interactHandler.OnClickGround += OnPlayerShoot;
        }

        private void OnDisable()
        {
            _inputData.OnSwitchWeaponSlot -= SwitchSlot;
            _interactHandler.OnClickGround -= OnPlayerShoot;
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

        private void OnPlayerShoot(Vector3 targetPosition)
        {
            WeaponShootInfo info = new(targetPosition);
            _weaponSlots[_selectedSlot].CurrentWeapon.TryShoot(info);
        }
    }
}