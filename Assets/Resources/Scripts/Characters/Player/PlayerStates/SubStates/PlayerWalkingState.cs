using Tcp4.Assets.Resources.Scripts.Characters.Player.PlayerStates.SuperStates;
using Tcp4.Assets.Resources.Scripts.Core;

namespace Tcp4.Assets.Resources.Scripts.Characters.Player.PlayerStates.SubStates
{
    public class PlayerWalkingState : PlayerGroundedState
    {
        protected override void ConfigureAnimation()
        {
            StateAnimation = new AnimationData(
                stateName: "PlayerRun",
                transitionDuration: 0
            );
        }

        public override void DoPhysicsLogic()
        {
            base.DoPhysicsLogic();
            Movement(InputHandler.GetRawMovementDirection());
        }
    }
}
