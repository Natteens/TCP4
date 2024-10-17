using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentUtils;
using System;

namespace Tcp4
{
    public class PlayerGroundedState : State
    {
        protected PlayerInputHandler InputHandler;
        protected CollisionComponent coll;
        protected IInteractable currentInteractable;

        public override void Initialize(DynamicEntity entity)
        {
            base.Initialize(entity);
            coll = entity.checker;
            InputHandler = entity.serviceLocator.GetService<PlayerInputHandler>();
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            HandleInput();
            CheckInteraction();
        }

        private void HandleInput()
        {
            if (coll.IsColliding<RaycastResult>("Ground", out var _))
            {
                if (InputHandler.GetRawMovementDirection() != Vector3.zero)
                {
                    entity.machine.ChangeState("Move", entity);
                }
                else
                {
                    entity.machine.ChangeState("Idle", entity);
                } 
            }

            if (InputHandler.GetInteractInput() && currentInteractable != null)
            {
                entity.machine.ChangeState("Interact", entity);
            }
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        protected void Movement(Vector3 input)
        {
            float speed = entity.statusComponent.GetStatus(StatusType.Speed);
            entity.movement.Move(input, speed);
        }

        private void CheckInteraction()
        {
            if (coll.IsColliding<EntityCollisionResult>("Interact", out var result))
            {
                var interactable = result.Entity as IInteractable;
                if (interactable != null && interactable != currentInteractable)
                {
                    currentInteractable?.OnPlayerExit();
                    currentInteractable = interactable;
                    currentInteractable.OnPlayerEnter();
                }
            }
            else if (currentInteractable != null)
            {
                currentInteractable.OnPlayerExit();
                currentInteractable = null;
            }
        }

    }

}

