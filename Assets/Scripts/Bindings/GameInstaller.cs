using UnityEngine;
using Void.Core.Events;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IEventBus>().To<EventBus>().FromNew().AsSingle().NonLazy();
    }
}