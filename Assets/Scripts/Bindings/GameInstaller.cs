using Input;
using UnityEngine;
using Void.Events;
using Void.States;
using Zenject;

namespace Void.Bindings
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private StateSettings stateSettings;
        [SerializeField] private UnityEngine.Camera viewCamera;
        public override void InstallBindings()
        {
            Container.Bind<IEventBus>().To<EventBus>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<InputController>()
                .FromNew()
                .AsSingle()
                .WithArguments(viewCamera)
                .NonLazy();

            Container.Bind<IStateDTO>().To<StateDTO>().FromInstance(new StateDTO(stateSettings)).AsSingle().NonLazy();
            Container.Bind<IState>().To<MainMenuState>().FromNew().AsSingle().NonLazy();
            Container.Bind<IState>().To<PeaceState>().FromNew().AsSingle().NonLazy();
            Container.Bind<IState>().To<BattleState>().FromNew().AsSingle().NonLazy();
            Container.Bind<IState>().To<GameOverState>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<StateController>()
                .FromNew()
                .AsSingle()
                .WithArguments(stateSettings.InitialState)
                .NonLazy();
        }
    }
}