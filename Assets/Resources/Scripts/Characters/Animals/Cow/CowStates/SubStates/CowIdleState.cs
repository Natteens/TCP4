using Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow.CowStates.SuperStates;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Animals.Cow
{
    public class CowIdleState : CowGroundedState
    {
        private float idleTimer;
        private float currentIdleTime;
        private const float MinIdleTime = 1f;
        private const float MaxIdleTime = 3f;


        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            Movement(Vector3.zero);
            idleTimer = 0f;
            currentIdleTime = Random.Range(MinIdleTime, MaxIdleTime);
        }

        public override void DoPhysicsLogic()
        {
            base.DoPhysicsLogic();
            idleTimer += Time.fixedDeltaTime;        
        }

        protected override void HandleMovementDecision()
        {
            if (!IsGrounded()) return;

            if (Pathfinding != null && !Pathfinding.HasReachedCurrentPoint &&
                idleTimer >= currentIdleTime &&
                HasReachedMinStateTime())
            {
                Pathfinding.MoveToNextPoint();
                Entity.Machine.ChangeState("Move", Entity);
            }
        }
    }
}