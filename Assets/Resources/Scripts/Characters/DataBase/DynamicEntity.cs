using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public abstract class DynamicEntity : BaseEntity
    {
        public StateMachine machine;
        public Movement movement;
        public event Action<AbilityType> OnAbilityUnlocked;

        public override void Awake()
        {
            base.Awake();
            movement = new Movement(this);
            machine = new StateMachine();
        }

        protected virtual void Update()
        {
            machine.CurrentState.DoFrameUpdateLogic();
        }

        protected virtual void FixedUpdate()
        {
            machine.CurrentState.DoPhysicsLogic();
        }

        public AbilitySet GetAbility()
        {
            return baseStatus.GetBaseAbilitySet();
        }

        protected void UnlockAbility(AbilityType ability)
        {
            baseStatus.abilitySet.SetAbilityValue(ability, true);
            OnAbilityUnlocked?.Invoke(ability);
        }
    }
}
