using Void.Events;

namespace Void.States
{
    public static class StateEvent
    {
        public class Change : IEvent
        {
            public StateId StateId { get; }
            
            public Change(StateId stateId)
            {
                StateId = stateId;
            }
        }
    }
}
