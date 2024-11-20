using Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow.CowStates.SuperStates;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow.CowStates.SubStates
{
    public class CowMovementState : CowGroundedState
    {
        private float currentWaitTime;
        private const float TransitionThreshold = 0.1f;

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            currentWaitTime = 0f;
        }

        public override void DoPhysicsLogic()
        {
            base.DoPhysicsLogic();
            if (Pathfinding == null) return;

            if (Pathfinding.HasReachedCurrentPoint)
            {
                HandleWaiting();
            }
            else
            {
                HandleMovement();
            }
        }

        private void HandleMovement()
        {
            Vector3 moveDirection = Pathfinding.GetMovementDirection();

            if (moveDirection.magnitude > 0)
            {
                Movement(moveDirection);  // Passa a direção para o método Movement do script Movement
            }
            else
            {
                Movement(Vector3.zero);  // Se não estiver se movendo, para a vaca
            }
        }

        private void HandleWaiting()
        {
            Movement(Vector3.zero);  // Para a vaca enquanto ela espera
            currentWaitTime += Time.deltaTime;

            if (currentWaitTime >= Pathfinding.CurrentWaitTime && HasReachedMinStateTime())
            {
                currentWaitTime = 0f;
                Pathfinding.MoveToNextPoint();
            }
        }


        protected override void HandleMovementDecision()
        {
            if (!IsGrounded()) return;

            if (Pathfinding.HasReachedCurrentPoint &&
                currentWaitTime >= Pathfinding.CurrentWaitTime &&
                HasReachedMinStateTime())
            {
                Entity.Machine.ChangeState("Idle", Entity);
            }
        }
    }
}
