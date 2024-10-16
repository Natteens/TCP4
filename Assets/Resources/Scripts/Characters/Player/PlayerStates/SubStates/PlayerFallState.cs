using UnityEngine;

namespace CrimsonReaper
{
    public class PlayerFallState : PlayerAirborneState
    {
        [SerializeField] private float maxFallSpeed = -15f;
        [SerializeField] private float significantFallDistance = 5f;

        private float maxFallDistance;

        protected override void ConfigureAnimationParameters()
        {
            animationParameters = new AnimationStateParameter[]
            {
            new BoolStateAnimationParameter("IsFalling", true, false),
            };
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            fallStartHeight = entity.transform.position.y;
            maxFallDistance = 0f;
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            ApplyFallGravity();
            CheckFallAndLanding();
        }

        private void CheckFallAndLanding()
        {
            float currentFallDistance = fallStartHeight - entity.transform.position.y;
            maxFallDistance = Mathf.Max(maxFallDistance, currentFallDistance);

            float distanceToGround = coll.CalculateFallDistance();
            Debug.Log($"Current Fall Distance: {currentFallDistance}, Max Fall Distance: {maxFallDistance}, Distance to Ground: {distanceToGround}");

            if (coll.IsColliding<RaycastResult>("Ground", out var _)) // Consideramos que est� no ch�o se a dist�ncia for muito pequena
            {
                if (maxFallDistance >= significantFallDistance)
                {
                    entity.machine.ChangeState("Land", entity);
                }
                else
                {
                    HandleImmediateLanding();
                }
            }
        }

        private void HandleImmediateLanding()
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

        private void ApplyFallGravity()
        {
            Vector3 velocity = entity.rb.linearVelocity;
            velocity.y = Mathf.Max(velocity.y + Physics.gravity.y * Time.deltaTime, maxFallSpeed);
            entity.rb.linearVelocity = velocity;
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }
    }
}
