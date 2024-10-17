using System;
using UnityEngine;
using ComponentUtils;

namespace Tcp4
{
    public class Player : DynamicEntity
    {
        private PlayerInputHandler Input;

        private PlayerIdleState idleState;
        private PlayerMovementState runState;
        private PlayerInteractState interactState;

        public override void Awake()
        {
            base.Awake();
            TryGetComponent<PlayerInputHandler>(out Input);
            serviceLocator.RegisterService<PlayerInputHandler>(Input);
        }

        private void Start()
        {
            OnAbilityUnlocked += HandleAbilityUnlocked;
            RegisterBaseStates();
            machine.Initialize(idleState);
        }

        private void RegisterBaseStates()
        {
            idleState = new PlayerIdleState();
            runState = new PlayerMovementState();
            interactState = new PlayerInteractState();

            machine.RegisterState("Idle", idleState,this,abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            machine.RegisterState("Move", runState,this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            machine.RegisterState("Interact", interactState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanInteract));
        }

        private void HandleAbilityUnlocked(AbilityType unlockedAbility)
        {
            UnlockAbility(unlockedAbility);
        }
    }
}
