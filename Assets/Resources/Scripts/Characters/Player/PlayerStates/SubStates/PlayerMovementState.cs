using UnityEngine;

namespace Tcp4
{
    public class PlayerMovementState : PlayerGroundedState
    {

        protected override void ConfigureAnimationParameters()
        {
            animationParameters = new AnimationStateParameter[]
            {
                new FloatStateAnimationParameter("Speed", 1f, 0f), 
            };
        }

        public override void DoPhysicsLogic()
        {
            base.DoPhysicsLogic();
            Movement(InputHandler.GetRawMovementDirection());
        }
    }
}
