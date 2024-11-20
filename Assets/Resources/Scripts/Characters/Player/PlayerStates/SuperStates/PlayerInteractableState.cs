using Tcp4.Resources.Scripts.FSM;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates
{
    public class PlayerInteractableState : State<Assets.Resources.Scripts.Characters.Player.Player>
    {
        protected PlayerInputHandler InputHandler;
        private bool isInteracting;

        public override void Initialize(Assets.Resources.Scripts.Characters.Player.Player e)
        {
            base.Initialize(e);
            InputHandler = Entity.ServiceLocator.GetService<PlayerInputHandler>();
        }
        

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            isInteracting = true;
            var interactable = Entity.InteractionManager.CurrentInteractable;
            interactable?.StartInteraction();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            if (isInteracting && Entity.InteractionManager.CurrentInteractable != null)
            {
                Entity.InteractionManager.CurrentInteractable.ExecuteInteraction();
            }
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
            if (isInteracting && Entity.InteractionManager.CurrentInteractable != null)
            {
                Entity.InteractionManager.CurrentInteractable.EndInteraction();
            }
            isInteracting = false;
        }
    }
}
