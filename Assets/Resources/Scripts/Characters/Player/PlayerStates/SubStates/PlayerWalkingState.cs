using Tcp4.Assets.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SubStates
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