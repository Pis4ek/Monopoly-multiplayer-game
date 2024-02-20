using System;
using System.Collections.Generic;

namespace MainMenu.StateMachine
{
    public class MainMenuStateMachine : IStateMachine<IMainMenuState>
    {
        private readonly Dictionary<Type, IMainMenuState> _registeredStates = new();
        public Type CurrentState { get; private set; }
        private Type _typeOfStartState;

        public bool RegisterState(IMainMenuState state, bool isStartState = false) 
        {
            if (_registeredStates.ContainsKey(state.GetType()))
            {
                UnityEngine.Debug.LogWarning($"This state is already registered: {state.GetType()}. Registration is rejected.");
                return false;
            }
            else 
            {
                _registeredStates.Add(state.GetType(), state);
                if (isStartState)
                    _typeOfStartState = state.GetType();
                return true;
            }
        }

        public void Start() 
        {
            foreach (IMainMenuState state in _registeredStates.Values) 
            {
                state.ResetState();
            }
            if (_typeOfStartState == null)
                UnityEngine.Debug.LogError($"{this.GetType()}: Start state is null");
            _registeredStates[_typeOfStartState].Enter();
            CurrentState = _typeOfStartState;
        }

        public void SwitchState<U>() where U : IMainMenuState
        {
            SwitchState(typeof(U));
        }

        private void SwitchState(Type stateType) 
        {
            if (_registeredStates.ContainsKey(stateType))
            {
                _registeredStates[CurrentState].Exit();
                _registeredStates[stateType].Enter();
                CurrentState = stateType;
            }
            else
                UnityEngine.Debug.LogWarning($"State is not registered: {stateType}");
        }
    }
}
