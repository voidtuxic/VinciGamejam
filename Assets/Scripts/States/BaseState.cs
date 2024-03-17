using Void.Events;

namespace Void.States
{
    public abstract class BaseState : IState
    {
        protected IEventBus EventBus { get; }
        protected IStateDTO Data { get; }
        public abstract StateId StateId { get; }
        
        protected BaseState(IEventBus eventBus, IStateDTO data)
        {
            Data = data;
            EventBus = eventBus;
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
        }
    }
}
