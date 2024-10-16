using UnityEngine;

namespace CrimsonReaper
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

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            Debug.Log("Player está no estado Walk.");
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();

            Movement(InputHandler.GetNormalizedDirectionX());
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }
    }
}
