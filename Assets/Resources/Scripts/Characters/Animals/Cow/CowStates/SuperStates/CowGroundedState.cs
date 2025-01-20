using Tcp4.Assets.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.FSM;
using Tcp4.Resources.Scripts.Systems.CollisionCasters;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Animals.Cow.CowStates.SuperStates
{
    public class CowGroundedState : State<Cow>
    {
        protected CollisionComponent Checker;
        protected NPCPathfinding Pathfinding;
        protected float StateTimer;
        protected const float MinStateTime = 0.5f;
        protected const float PathUpdateCooldown = 0.1f;
        private float lastPathUpdate;

        public override void Initialize(Cow entity)
        {
            base.Initialize(entity);
            Checker = entity.Checker;
            Pathfinding = entity.ServiceLocator.GetService<NPCPathfinding>();
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            StateTimer = 0f;
        }

        public override void DoPhysicsLogic()
        {
            base.DoPhysicsLogic();
            StateTimer += Time.fixedDeltaTime;

            if (!IsGrounded()) return;
            
            if (Time.time >= lastPathUpdate + PathUpdateCooldown)
            {
                HandleMovementDecision();
                lastPathUpdate = Time.time;
            }
        }

        protected virtual void HandleMovementDecision()
        {
            if (Pathfinding == null) return;
            
            if (Pathfinding.HasReachedCurrentPoint)
            {
                if (HasReachedMinStateTime())
                {
                    Entity.Machine.ChangeState("Idle", Entity);
                }
            }
            else if (HasReachedMinStateTime())
            {
                Entity.Machine.ChangeState("Move", Entity);
            }

            if (Entity.IsInteracting())
            {
                Pathfinding.StopMoving();
            }
            else
            {
                Pathfinding.StartMoving();
            }
        }

        protected void Movement(Vector3 input)
        {
            float speed = Entity.StatusComp.GetStatus(StatusType.Speed);
            Entity.Movement.Move(input, speed);
        }

        protected bool HasReachedMinStateTime()
        {
            return StateTimer >= MinStateTime;
        }

        protected bool IsGrounded()
        {
            return Checker.IsColliding<CollisionResult>("Ground", out var _);
        }
    }
}
