using System;

namespace SiegeStorm.WeaponSystem
{
    public class WeaponSlot
    {
        public event Action<Weapon> OnWeaponChanged;
        public event Action<bool> OnSelectStateChanged;

        public Weapon CurrentWeapon { get; private set; }

        public void SetWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            OnWeaponChanged?.Invoke(weapon);
        }

        public void OnSelect()
        {
            OnSelectStateChanged?.Invoke(true);
        }

        public void OnDeselect()
        {
            OnSelectStateChanged?.Invoke(false);
        }
    }
}