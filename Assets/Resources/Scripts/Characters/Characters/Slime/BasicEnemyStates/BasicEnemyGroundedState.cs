using ComponentUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonReaper
{
    public class BasicEnemyGroundedState : State
    {
        protected CollisionComponent coll;
        protected float minTimeInState = 2f;
        protected float stateTimer;
        protected int currentDirection;

        [SerializeField] protected float directionChangeDelay = 0.5f;
        private float lastDirectionChangeTime;

        protected override void ConfigureAnimationParameters() { }

        public override void Initialize(DynamicEntity entity)
        {
            base.Initialize(entity);
            coll = entity.checker;
            ChooseRandomDirection();
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            stateTimer = 0f;
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            stateTimer += Time.deltaTime;
            HandleDirectionChange();
            HandleStateTransitions();
        }

        protected virtual void HandleDirectionChange()
        {
            if (ShouldChangeDirection() && Time.time - lastDirectionChangeTime >= directionChangeDelay)
            {
                ChangeDirection();
                lastDirectionChangeTime = Time.time;
            }
        }

        protected bool ShouldChangeDirection()
        {
            return coll.IsColliding<RaycastResult>("Wall", out var _) || !coll.IsColliding<RaycastResult>("Edge", out var _);
        }

        protected void ChangeDirection()
        {
            currentDirection *= -1;
        }

        protected void ChooseRandomDirection()
        {
            currentDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        }

        protected virtual void HandleStateTransitions()
        {
            if (stateTimer >= minTimeInState)
            {
                if (ShouldTransitionToPatrol())
                {
                    entity.machine.ChangeState("Patrol", entity);
                }
                else if (ShouldTransitionToIdle())
                {
                    entity.machine.ChangeState("Idle", entity);
                }
            }
        }

        protected virtual bool ShouldTransitionToPatrol()
        {
            return Random.Range(0, 100) < 50;
        }

        protected virtual bool ShouldTransitionToIdle()
        {
            return Random.Range(0, 100) < 50;
        }

        protected void Movement(int dir)
        {
            float speed = entity.statusComponent.GetStatus(StatusType.Speed);
            entity.movement.Move(dir, speed);
        }
    }
}
