using Tcp4.Assets.Resources.Scripts.Characters.Player.PlayerStates.SubStates;
using Tcp4.Assets.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.Characters.Player;
using Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SubStates;
using Tcp4.Resources.Scripts.Core;

namespace Tcp4.Assets.Resources.Scripts.Characters.Player
{
    public class Player : DynamicEntity
    {
        private PlayerInputHandler _input;

        private PlayerIdleState _idleState;
        private PlayerWalkingState _walkState;
        private PlayerRunningState _runState;
        private PlayerTalkState _talkState;
        private PlayerHarvestState _harvestState;


        public override void Awake()
        {
            base.Awake();
            Movement = new Movement(this);
            TryGetComponent(out _input);
            ServiceLocator.RegisterService(_input);
        }

        private void Start()
        {
            OnAbilityUnlocked += HandleAbilityUnlocked;
            RegisterBaseStates();
            Machine.Initialize(_idleState);
        }

        private void RegisterBaseStates()
        {
            _idleState = new PlayerIdleState();
            _walkState = new PlayerWalkingState();
            _runState = new PlayerRunningState();
            _talkState = new PlayerTalkState();
            _harvestState = new PlayerHarvestState();

            Machine.RegisterState("Idle", _idleState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            Machine.RegisterState("Run", _runState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            Machine.RegisterState("Walk", _walkState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            Machine.RegisterState("Talk", _talkState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanInteract));
            Machine.RegisterState("Harvest", _harvestState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanInteract));

        }

        private void HandleAbilityUnlocked(AbilityType unlockedAbility)
        {
            UnlockAbility(unlockedAbility);
        }
    }
}
