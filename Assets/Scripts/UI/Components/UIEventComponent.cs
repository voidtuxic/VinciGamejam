using Void.Events;
using UnityEngine;
using Zenject;

namespace Void.UI.Components
{
    public abstract class UIEventComponent : MonoBehaviour
    {
        protected IEventBus EventBus { get; private set; }
    
        [Inject]
        private void Construct(IEventBus eventBus)
        {
            EventBus = eventBus;
            Bind();
        }

        protected virtual void Bind() { }
    }
}
