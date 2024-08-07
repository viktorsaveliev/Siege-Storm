using SiegeStorm.PlayerController;
using SiegeStorm.UISystem;
using SiegeStorm.WeaponSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private WeaponRadiusUI _radiusUI;
    [SerializeField] private LayerMask _layerMask;

    private PlayerWeapon _playerWeapon;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _playerWeapon.OnWeaponChanged += OnWeaponChanged;
    }

    private void OnDisable()
    {
        _playerWeapon.OnWeaponChanged -= OnWeaponChanged;
    }

    private void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, MouseRaycaster.MAX_RAY_DISTANCE, _layerMask))
        {
            Vector3 position = hit.point;
            position.y += 0.1f;

            _radiusUI.SetPosition(position);
        }
    }

    [Inject]
    public void Construct(PlayerWeapon playerWeapon)
    {
        _playerWeapon = playerWeapon;
    }

    private void OnWeaponChanged(Weapon weapon)
    {
        ProjectileData data = weapon.Data.Projectile.Data;
        if (data.IsRadiusDamage)
        {
            _radiusUI.SetRadius(data.Radius);
        }
        else
        {
            _radiusUI.SetActive(false);
        }
    }

    
}
