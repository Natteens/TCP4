using System;
using UnityEngine;
using ComponentUtils;

namespace CrimsonReaper
{
    public class Player : DynamicEntity
    {
        private PlayerInputHandler Input;

        private PlayerIdleState idleState;
        private PlayerMovementState runState;
        private PlayerJumpState jumpState;
        private PlayerFallState fallState;
        private PlayerLandingState landingState;
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
            jumpState = new PlayerJumpState();
            fallState = new PlayerFallState();
            landingState = new PlayerLandingState();
            interactState = new PlayerInteractState();

            machine.RegisterState("Idle", idleState,this,abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            machine.RegisterState("Move", runState,this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            machine.RegisterState("Jump", jumpState,this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanAirborne));
            machine.RegisterState("Fall", fallState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanAirborne));
            machine.RegisterState("Land", landingState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanAirborne));
            machine.RegisterState("Interact", interactState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanInteract));
        }


        private void HandleAbilityUnlocked(AbilityType unlockedAbility)
        {
            Debug.Log("Habilidade desbloqueada: " + unlockedAbility);
        }
    }
}
