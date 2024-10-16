using UnityEngine;
using System;
using System.Collections.Generic;

namespace CrimsonReaper
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        private Dictionary<string, IState> states;
        private Dictionary<string, Func<AbilitySet, bool>> stateAbilities;

        public StateMachine()
        {
            states = new Dictionary<string, IState>();
            stateAbilities = new Dictionary<string, Func<AbilitySet, bool>>();
        }

        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
           // Debug.Log($"Estado inicial configurado: {startingState?.GetType().Name}");
            CurrentState?.DoEnterLogic();
        }


        public void ChangeState(string newStateName, DynamicEntity entity)
        {
            if (states.TryGetValue(newStateName, out var newState) && CanSwitchState(newStateName, entity))
            {
                CurrentState.DoExitLogic();
                CurrentState = newState;
                CurrentState.DoEnterLogic();
            }
            else
            {
                Debug.LogWarning($"Não foi possível mudar para o estado: {newStateName}. Estado atual: {CurrentState?.GetType().Name}");
            }
        }

        public void UpdateState()
        {
            CurrentState?.DoFrameUpdateLogic();
        }

        public void PhysicsUpdateState()
        {
            CurrentState?.DoPhysicsLogic();
        }

        public void RegisterState(string stateName, IState state, DynamicEntity entity, Func<AbilitySet, bool> canSwitchFunc = null)
        {
            if (!states.ContainsKey(stateName))
            {
                states[stateName] = state;
                state.Initialize(entity);

                if (canSwitchFunc != null)
                {
                    stateAbilities[stateName] = canSwitchFunc;
                }
            }
            else
            {
                Debug.LogWarning($"Estado já registrado: {stateName}");
            }
        }

        private bool CanSwitchState(string newStateName, DynamicEntity entity)
        {
            if (stateAbilities.TryGetValue(newStateName, out var canSwitchFunc))
            {
                return canSwitchFunc(entity.GetAbility());
            }
            return false;
        }
    }
}
