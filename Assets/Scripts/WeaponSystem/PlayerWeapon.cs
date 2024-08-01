using SiegeStorm.PlayerController;
using SiegeStorm.UISystem;
using UnityEngine;
using Zenject;

namespace SiegeStorm.WeaponSystem
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Weapon[] _startedWeapons = new Weapon[MAX_WEAPON_SLOTS];
        [SerializeField] private WeaponSlotUI[] _slotsUI = new WeaponSlotUI[MAX_WEAPON_SLOTS];

        private const int MAX_WEAPON_SLOTS = 3;
        private readonly WeaponSlot[] _weaponSlots = new WeaponSlot[MAX_WEAPON_SLOTS];

        private IInteractHandler _interactHandler;
        private int _selectedSlot;

        private void Awake()
        {
            for(int i = 0; i < MAX_WEAPON_SLOTS; i++)
            {
                _weaponSlots[i] = new();
                _weaponSlots[i].SetWeapon(_startedWeapons[i]);

                _slotsUI[i].Init(_weaponSlots[i]);
            }

            _selectedSlot = 1;
        }

        private void OnEnable()
        {
            _interactHandler.OnClickGround += OnPlayerShoot;
        }

        private void OnDisable()
        {
            _interactHandler.OnClickGround -= OnPlayerShoot;
        }

        [Inject]
        public void Construct(IInteractHandler interactHandler)
        {
            _interactHandler = interactHandler;
        }

        private void OnPlayerShoot(Vector3 targetPosition)
        {
            WeaponShootInfo info = new(targetPosition);
            _weaponSlots[_selectedSlot].CurrentWeapon.TryShoot(info);
        }
    }
}