using UnityEngine;

namespace CrimsonReaper
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
         //   Debug.Log("Player está no estado Idle.");
            Movement(0);
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }
    }
}
