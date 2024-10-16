using UnityEngine;

namespace CrimsonReaper
{
    public class PlayerJumpState : PlayerAirborneState
    {
        [SerializeField] private float minJumpVelocity = 5f;
        [SerializeField] private float maxJumpVelocity = 8f;
        [SerializeField] private float upwardMovementMultiplier = 1f;
        [SerializeField] private float downwardMovementMultiplier = 1.7f;

        private bool hasJumped;
        private bool isJumping;

        protected override void ConfigureAnimationParameters()
        {
            animationParameters = new AnimationStateParameter[]
            {
                new BoolStateAnimationParameter("IsJumping", true, false),
            };
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            hasJumped = false;
            isJumping = true;
            Jump();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();

            if (isJumping && !InputHandler.IsJumpButtonHeld())
            {
                if (entity.rb.linearVelocity.y > 0)
                {
                    entity.rb.linearVelocity = new Vector2(entity.rb.linearVelocity.x, entity.rb.linearVelocity.y * 0.5f);
                }
                isJumping = false;
            }
            if (entity.rb.linearVelocity.y < 0)
            {
                entity.machine.ChangeState("Fall", entity);
            }
        }

        private void Jump()
        {
            if (hasJumped) return;
            float jumpHoldTimeNormalized = InputHandler.GetJumpHoldTime() / 0.5f;
            jumpHoldTimeNormalized = Mathf.Clamp01(jumpHoldTimeNormalized);
            float currentJumpVelocity = Mathf.Lerp(minJumpVelocity, maxJumpVelocity, jumpHoldTimeNormalized);
            entity.rb.linearVelocity = new Vector2(entity.rb.linearVelocity.x, currentJumpVelocity);
            hasJumped = true;
        }


    }
}