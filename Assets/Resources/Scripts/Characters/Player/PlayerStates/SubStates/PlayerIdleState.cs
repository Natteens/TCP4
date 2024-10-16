using UnityEngine;

namespace Tcp4
{
    public class PlayerIdleState : PlayerGroundedState
    {
        protected override void ConfigureAnimationParameters()
        {
            animationParameters = new AnimationStateParameter[]
            {
                new FloatStateAnimationParameter("Speed", 0f, 0f),
            };
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            Movement(0);
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }
    }
}
