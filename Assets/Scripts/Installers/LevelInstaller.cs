using SiegeStorm.Destructibility;
using SiegeStorm.InputSystem;
using SiegeStorm.PlayerController;
using SiegeStorm.WeaponSystem;
using UnityEngine;
using Zenject;

namespace SiegeStorm
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private MouseRaycaster _interactHandler;
        [SerializeField] private PlayerWeapon _playerWeapon;
        [SerializeField] private CrackSystem _crackSystem;
        [SerializeField] private CameraShake _cameraShake;

        private InputData _inputData;

        private void Awake()
        {
            Resolve();

            _inputData.Init();
        }

        public override void InstallBindings()
        {
            Container.Bind<InputData>().FromNew().AsSingle();

            Container.Bind<IInteractHandler>().FromInstance(_interactHandler).AsSingle();
            Container.Bind<PlayerWeapon>().FromInstance(_playerWeapon).AsSingle();
            Container.Bind<CrackSystem>().FromInstance(_crackSystem).AsSingle();
            Container.Bind<CameraShake>().FromInstance(_cameraShake).AsSingle();
        }

        private void Resolve()
        {
            _inputData = Container.Resolve<InputData>();
        }
    }
}
