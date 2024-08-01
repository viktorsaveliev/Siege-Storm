using SiegeStorm.InputSystem;
using SiegeStorm.PlayerController;
using UnityEngine;
using Zenject;

namespace SiegeStorm
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private MouseRaycaster _interactHandler;

        private InputData _inputData;

        private void Awake()
        {
            Resolve();

            _inputData.Init(this);
        }

        public override void InstallBindings()
        {
            Container.Bind<InputData>().FromNew().AsSingle();

            Container.Bind<IInteractHandler>().FromInstance(_interactHandler).AsSingle();
        }

        private void Resolve()
        {
            _inputData = Container.Resolve<InputData>();
        }
    }
}
