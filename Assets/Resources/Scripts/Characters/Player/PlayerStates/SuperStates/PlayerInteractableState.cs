using Tcp4.Resources.Scripts.FSM;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates
{
    public class PlayerInteractableState : State<Player>
    {
        protected PlayerInputHandler InputHandler;
        protected CollisionComponent Checker;
        
        public override void Initialize(Player entity)
        {
            base.Initialize(entity);
            Checker = entity.Checker;
            InputHandler = entity.ServiceLocator.GetService<PlayerInputHandler>();
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
           
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }
    }

}
