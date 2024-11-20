using Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow.CowStates.SubStates;
using Tcp4.Assets.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.Characters.Animals.Cow;
using Tcp4.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.Systems.Interaction;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow
{
    public class Cow : DynamicEntity
    {
        private NPCPathfinding Pathfinding;
        private InteractableComponent interactableComp;

        private CowIdleState _idleState;
        private CowMovementState _movementState;
        private CowTalkState _talkState;

        public override void Awake()
        {
            base.Awake();
            Movement = new Movement(this);
            
            TryGetComponent<NPCPathfinding>(out Pathfinding);
            ServiceLocator.RegisterService<NPCPathfinding>(Pathfinding);

            TryGetComponent<InteractableComponent>(out interactableComp);
            ServiceLocator.RegisterService<InteractableComponent>(interactableComp);
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
            _talkState = new CowTalkState();

            Machine.RegisterState("Idle", _idleState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            Machine.RegisterState("Move", _movementState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            Machine.RegisterState("Talk", _talkState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanInteract));
        }
    }
}
