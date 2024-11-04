using Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SubStates;
using Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates;
using Tcp4.Resources.Scripts.Core;

namespace Tcp4.Resources.Scripts.Characters.Player
{
    public class Player : DynamicEntity
    {
        private PlayerInputHandler _input;
        
        private PlayerIdleState _idleState;
        private PlayerMovementState _runState;
        private PlayerTalkState _talkState;
        private PlayerHarvestState _harvestState;
        

        public override void Awake()
        {
            base.Awake();
            TryGetComponent<PlayerInputHandler>(out _input);
            ServiceLocator.RegisterService<PlayerInputHandler>(_input);
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
            _runState = new PlayerMovementState();
            _talkState = new PlayerTalkState();
            _harvestState = new PlayerHarvestState();

            Machine.RegisterState("Idle", _idleState,this,abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            Machine.RegisterState("Move", _runState,this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            Machine.RegisterState("Talk", _talkState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanInteract));
            Machine.RegisterState("Harvest", _harvestState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanInteract));
            
        }

        private void HandleAbilityUnlocked(AbilityType unlockedAbility)
        {
            UnlockAbility(unlockedAbility);
        }
    }
}
