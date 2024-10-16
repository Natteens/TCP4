using UnityEngine;

namespace CrimsonReaper
{
    public class PlayerLandingState : PlayerGroundedState
    {
        [SerializeField] private float landingRecoveryTime = 0.5f;
        private float recoveryTimer;
        private bool canExitEarly = true;

        protected override void ConfigureAnimationParameters()
        {
            animationParameters = new AnimationStateParameter[]
            {
                new BoolStateAnimationParameter("IsLanding", true, false)
            };
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            recoveryTimer = landingRecoveryTime;
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();

            recoveryTimer -= Time.deltaTime;

            if (canExitEarly && InputHandler.GetNormalizedDirectionX() != 0)
            {
                TransitionToNextState();
                return;
            }

            if (recoveryTimer <= 0)
            {
                TransitionToNextState();
            }
        }

        private void TransitionToNextState()
        {
            if (InputHandler.GetNormalizedDirectionX() != 0)
            {
                entity.machine.ChangeState("Move", entity);
            }
            else
            {
                entity.machine.ChangeState("Idle", entity);
            }
        }
    }
}
