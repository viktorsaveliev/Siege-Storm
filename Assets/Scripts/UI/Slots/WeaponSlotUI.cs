using UnityEngine;
using TMPro;
using SiegeStorm.WeaponSystem;
using DG.Tweening;
using UnityEngine.UI;

namespace SiegeStorm.UISystem
{
    [RequireComponent(typeof(Button))]
    public class WeaponSlotUI : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _reloadIndicator;
        [SerializeField] private TMP_Text _bulletsCount;

        private WeaponSlot _slot;

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        private void OnDestroy()
        {
            _slot.OnWeaponChanged -= SetWeapon;
            _slot.OnSelectStateChanged -= OnStateChanged;

            if (_slot.CurrentWeapon != null)
            {
                _slot.CurrentWeapon.OnReloadStart -= HandleReloadStart;
                _slot.CurrentWeapon.OnReloadEnd -= HandleReloadEnd;
                _slot.CurrentWeapon.OnFired -= UpdateBulletsCount;
            }
        }

        public void Init(WeaponSlot slot)
        {
            _slot = slot;
            _slot.OnWeaponChanged += SetWeapon;
            _slot.OnSelectStateChanged += OnStateChanged;

            if (_slot.CurrentWeapon != null)
            {
                SetWeapon(_slot.CurrentWeapon);
            }
        }

        private void OnStateChanged(bool selected)
        {
            float duration = 0.5f;

            if(selected)
            {
                _button.image.rectTransform.DOScale(new Vector2(1.2f, 1.2f), duration);
            }
            else
            {
                _button.image.rectTransform.DOScale(new Vector2(1f, 1f), duration);
            }
        }

        private void SetWeapon(Weapon weapon)
        {
            if(_slot.CurrentWeapon != null)
            {
                _slot.CurrentWeapon.OnReloadStart -= HandleReloadStart;
                _slot.CurrentWeapon.OnReloadEnd -= HandleReloadEnd;
                _slot.CurrentWeapon.OnFired -= UpdateBulletsCount;
            }

            _icon.sprite = weapon.Data.Icon;

            _slot.CurrentWeapon.OnReloadStart += HandleReloadStart;
            _slot.CurrentWeapon.OnReloadEnd += HandleReloadEnd;
            _slot.CurrentWeapon.OnFired += UpdateBulletsCount;

            UpdateBulletsCount();
        }

        private void HandleReloadStart()
        {
            _reloadIndicator.fillAmount = 0f;
            _reloadIndicator.enabled = true;

            _reloadIndicator.DOFillAmount(1f, _slot.CurrentWeapon.Data.ReloadDuration).SetEase(Ease.Linear)
                .OnComplete(() => _reloadIndicator.enabled = false);
        }

        private void HandleReloadEnd()
        {
            UpdateBulletsCount();
        }

        private void UpdateBulletsCount()
        {
            _bulletsCount.text = $"{_slot.CurrentWeapon.CurrentBulletsCount} / {_slot.CurrentWeapon.Data.BulletsInMagazine}";
        }
    }
}