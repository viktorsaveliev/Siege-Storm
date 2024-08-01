using System;

namespace SiegeStorm.WeaponSystem
{
    public class WeaponSlot
    {
        public event Action<Weapon> OnWeaponChanged;

        public Weapon CurrentWeapon { get; private set; }

        public void SetWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            CurrentWeapon.Init();

            OnWeaponChanged?.Invoke(weapon);
        }
    }
}