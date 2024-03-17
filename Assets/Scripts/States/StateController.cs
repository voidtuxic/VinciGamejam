using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Void.Events;

namespace Void.States
{
    public class StateController : IStateController
    {
        private readonly IEventBus _eventBus;
        private readonly Dictionary<StateId, IState> _states;
        private IState _currentState;

        public StateController(IEventBus eventBus, List<IState> states, StateId initialState)
        {
            _eventBus = eventBus;
            _states = states.ToDictionary(s => s.StateId, s => s);
            
            _eventBus.SubscribeEvent<StateEvent.Change>(OnStateChangeEvent);
            _eventBus.PublishEvent(new StateEvent.Change(initialState));
        }

        private void OnStateChangeEvent(StateEvent.Change evt)
        {
            Debug.Log($"Change state to {evt.StateId}");
            _currentState?.Exit();
            _currentState = _states[evt.StateId];
            _currentState.Enter();
        }

        public void Tick()
        {
            _currentState?.Update();
        }

        public void Dispose()
        {
            _eventBus.UnsubscribeEvent<StateEvent.Change>(OnStateChangeEvent);
        }
    }
}
