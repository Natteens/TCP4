using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentUtils;
using System;

namespace CrimsonReaper
{
    public class PlayerGroundedState : State
    {
        protected PlayerInputHandler InputHandler;
        protected CollisionComponent coll;
        protected IInteractable currentInteractable;

        protected override void ConfigureAnimationParameters()
        {
            animationParameters = new AnimationStateParameter[]
            {
                new BoolStateAnimationParameter("IsGrounded", true, false),
            };
        }

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
                if (InputHandler.GetNormalizedDirectionX() != 0)
                {
                    entity.machine.ChangeState("Move", entity);
                }
                else
                {
                    entity.machine.ChangeState("Idle", entity);
                } 
            }

            if (InputHandler.GetJumpInput())
            {
                entity.machine.ChangeState("Jump", entity);
            }
            else if (entity.rb.linearVelocity.y < 0 && !coll.IsColliding<RaycastResult>("Ground", out var _))
            {
                entity.machine.ChangeState("Fall", entity);
            }

            if (InputHandler.GetInteractInput() && currentInteractable != null)
            {
                Debug.Log("Interagindo com objeto");
                entity.machine.ChangeState("Interact", entity);
            }
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        protected void Movement(int input)
        {
            float speed = entity.statusComponent.GetStatus(StatusType.Speed);
          //  Debug.Log($"Speed atual: {speed}");
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

