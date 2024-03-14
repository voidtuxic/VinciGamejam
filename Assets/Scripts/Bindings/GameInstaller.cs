using Void.Events;
using Zenject;

namespace Void.Bindings
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEventBus>().To<EventBus>().FromNew().AsSingle().NonLazy();
        }
    }
}