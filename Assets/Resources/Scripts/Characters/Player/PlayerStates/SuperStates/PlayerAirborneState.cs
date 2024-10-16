using UnityEngine;

namespace CrimsonReaper
{
    public class PlayerAirborneState : State
    {
        protected PlayerInputHandler InputHandler;
        protected CollisionComponent coll;
        protected float coyoteTime = 0.2f;
        protected float coyoteTimeCounter;

        protected float fallStartHeight;
        protected bool hasFallStartHeightBeenSet;

        public override void Initialize(DynamicEntity entity)
        {
            base.Initialize(entity);
            coll = entity.checker;
            InputHandler = entity.serviceLocator.GetService<PlayerInputHandler>();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            HandleCoyoteTime();
            HandleAirMovement();
        }

        protected virtual void HandleAirMovement()
        {
            Movement(InputHandler.GetNormalizedDirectionX());
        }

        protected void Movement(int input)
        {
            float speed = entity.statusComponent.GetStatus(StatusType.Speed);
            entity.movement.Move(input, speed);
        }

        protected void HandleCoyoteTime()
        {
            if (coll.IsColliding<RaycastResult>("Ground", out var _))
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
        }

    }
}