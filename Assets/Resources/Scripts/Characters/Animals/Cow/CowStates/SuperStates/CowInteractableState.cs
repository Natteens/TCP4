using System.Collections;
using Tcp4.Assets.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.Characters.Animals.Cow;
using Tcp4.Resources.Scripts.FSM;
using Tcp4.Resources.Scripts.Systems.Interaction;
using UnityEngine;
using DG.Tweening;

namespace Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow.CowStates.SuperStates
{
    public class CowInteractableState : State<Cow>
    {
        protected Transform playerTransform;
        protected InteractableComponent interactableComponent;
        protected NPCPathfinding Pathfinding;
        protected bool isFacingPlayer = false;
        private bool isInteracting;

        private bool isRotating;
        private Tween currentRotationTween;
        private float rotationDuration = 0.5f; 

        public override void Initialize(Cow entity)
        {
            base.Initialize(entity);
            Pathfinding = entity.ServiceLocator.GetService<NPCPathfinding>();
            interactableComponent = entity.ServiceLocator.GetService<InteractableComponent>();
            interactableComponent.OnInteractionEnd += OnInteractionEnd;
        }

        public override void DoChecks()
        {
            base.DoChecks();
            isFacingPlayer = false;
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            isInteracting = true;
            Pathfinding.StopMoving();
            interactableComponent?.StartInteraction();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();

            if (isInteracting)
            {
                if (Entity.Checker.IsColliding<EntityCollisionResult>("Interact", out var result))
                {
                    playerTransform = result.HitObject.transform;
                }
                if (!isFacingPlayer)
                {
                    RotateTowardsPlayer();
                }
                interactableComponent?.ExecuteInteraction();
            }
        }

        private void RotateTowardsPlayer()
        {
            Debug.Log(playerTransform);
            if (playerTransform == null) return;
            Debug.Log(playerTransform);
            Debug.Log("Rotacioando pro player");
            Vector3 directionToPlayer = (playerTransform.position - Entity.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

            if (!isRotating)
            {
                float angle = Quaternion.Angle(Entity.transform.rotation, targetRotation);

                if (angle > 1f)
                {
                    isRotating = true;
                    currentRotationTween?.Kill(); 
                    currentRotationTween = Entity.transform
                        .DORotateQuaternion(targetRotation, rotationDuration)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() => isRotating = false);
                }
            }
            else
            {
                if (Quaternion.Angle(Entity.transform.rotation, targetRotation) <= 1f)
                {
                    isFacingPlayer = true;
                }
            }
        }

        private void OnInteractionEnd()
        {
            isInteracting = false;
            Debug.Log("Interação com a vaca finalizada.");
            Entity.Machine.ChangeState("Idle", Entity);
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
            if (isInteracting)
            {
                interactableComponent.EndInteraction();
                isInteracting = false;
            }
            Pathfinding.StartMoving();
            Pathfinding.MoveToNextPoint();
            Debug.Log("Saiu do estado de interação com a vaca.");
        }

        protected void Movement(Vector3 input)
        {
            float speed = Entity.StatusComp.GetStatus(StatusType.Speed);
            Entity.Movement.Move(input, speed);
        }
    }
}
