using Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow.CowStates.SubStates;
using Tcp4.Assets.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.Interfaces;
using Tcp4.Resources.Scripts.Systems.Interaction;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Animals.Cow
{
    public class Cow : DynamicEntity, IInteractable
    {
        [SerializeField]private bool canInteract = true;
        private bool isInteracting;
        private NPCPathfinding Pathfinding;

        private CowIdleState _idleState;
        private CowMovementState _movementState;

        public override void Awake()
        {
            base.Awake();
            Movement = new Movement(this);
            
            TryGetComponent<NPCPathfinding>(out Pathfinding);
            ServiceLocator.RegisterService<NPCPathfinding>(Pathfinding);
        }

        private void OnEnable()
        {
            InteractionEvents.OnInteractionStarted += HandleInteractionStarted;
            InteractionEvents.OnInteractionEnded += OnInteractionEnded;
        }
        
        private void OnDisable()
        {
            InteractionEvents.OnInteractionStarted -= HandleInteractionStarted;
            InteractionEvents.OnInteractionEnded -= OnInteractionEnded;
        }
        private void Start()
        {
            RegisterBaseStates();
            Machine.Initialize(_idleState);
        }

        private void RegisterBaseStates()
        {
            _idleState = new CowIdleState();
            _movementState = new CowMovementState();

            Machine.RegisterState("Idle", _idleState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            Machine.RegisterState("Move", _movementState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
        }
        
        private void HandleInteractionStarted(IInteractable interactable, BaseEntity interactor)
        {
            if (interactable == this)
            {
                Interact(interactor);
            }
        }
        
        public void Interact(BaseEntity interactor)
        {
            if (!canInteract) return;
                isInteracting = true;
       }

        private void OnInteractionEnded()
        {
            canInteract = true;
            isInteracting = false;
        }
        public bool IsInteracting() => isInteracting;
    }
}
