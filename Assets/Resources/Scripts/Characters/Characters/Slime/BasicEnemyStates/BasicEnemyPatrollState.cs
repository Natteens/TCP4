using UnityEngine;

namespace CrimsonReaper
{
    public class BasicEnemyPatrollState : BasicEnemyGroundedState
    {
        [SerializeField] private float patrolSpeedMultiplier = 1f;

        protected override void ConfigureAnimationParameters()
        {
            animationParameters = new AnimationStateParameter[]
            {
                new FloatStateAnimationParameter("Speed", 1f, 0f),
            };
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            Movement(currentDirection);
        }

        protected override bool ShouldTransitionToIdle()
        {
            return base.ShouldTransitionToIdle() || Random.Range(0, 100) < 30;
        }
    }
}
