using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonReaper
{
    public class BasicEnemyIdleState : BasicEnemyGroundedState
    {
        protected override void ConfigureAnimationParameters()
        {
            animationParameters = new AnimationStateParameter[]
            {
                new FloatStateAnimationParameter("Speed", 0f, 0f),
            };
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            Movement(0);
        }

        protected override bool ShouldTransitionToPatrol()
        {
            return base.ShouldTransitionToPatrol() || Random.Range(0, 100) < 30;
        }
    }
}
