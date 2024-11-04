using System;
using Tcp4.Resources.Scripts.FSM;
using Tcp4.Resources.Scripts.Interfaces;
using Tcp4.Resources.Scripts.Types;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates
{
    public class PlayerGroundedState : State<Player>
    {
        protected PlayerInputHandler InputHandler;
        protected CollisionComponent Checker;
        
        public override void Initialize(Player entity)
        {
            base.Initialize(entity);
            Checker = entity.Checker;
            InputHandler = entity.ServiceLocator.GetService<PlayerInputHandler>();
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
            if (Checker.IsColliding<SphereCollisionResult>("Ground", out var _))
            {
                if (InputHandler.GetRawMovementDirection() != Vector3.zero)
                {
                    Entity.Machine.ChangeState("Move", Entity);
                }
                else
                {
                    Entity.Machine.ChangeState("Idle", Entity);
                } 
            }

            if (InputHandler.GetInteractInput() && Entity.InteractionManager.CurrentInteractable != null)
            {
                InteractionType interactionType = Entity.InteractionManager.CurrentInteractable.InteractionKey;
                string interactionState = interactionType.ToString();
                Entity.Machine.ChangeState(interactionState, Entity);
            }
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        protected void Movement(Vector3 input)
        {
           // Debug.Log(input);
            float speed = Entity.StatusComp.GetStatus(StatusType.Speed);
            Entity.Movement.Move(input, speed);
        }

        private void CheckInteraction()
        {
            if (Checker.IsColliding<EntityCollisionResult>("Interact", out var entityResult))
            {
                Debug.Log(entityResult.HitObject.ToString());
                IInteractable interactable = entityResult.Entity.GetComponent<IInteractable>();
                HandleInteractableDetection(interactable, entityResult.Entity.gameObject);
            }
            else if (Checker.IsColliding<BoxCollisionResult>("Interact", out var result))
            {
                if (result.HitObject != null)
                {
                    Debug.Log(result.HitObject.ToString());
                    IInteractable interactable = result.HitObject.transform.gameObject.GetComponent<IInteractable>();
                    HandleInteractableDetection(interactable, result.HitObject.transform.gameObject);
                }
                else
                {
                    Debug.Log(" ta null como obj");
                    Entity.InteractionManager.SetInteractable(null);
                }
            }
            else
            {
                Debug.Log(" ta null como entidade");
                Entity.InteractionManager.SetInteractable(null);
            }
        }

        private void HandleInteractableDetection(IInteractable interactable, GameObject detectedObject)
        {
            if (interactable != null)
            {
                Debug.Log($"Interagível detectado: {detectedObject.name}");
                Entity.InteractionManager.SetInteractable(interactable);
                
            }
            else
            {
                Debug.LogWarning($"Objeto {detectedObject.name} não possui um componente IInteractable.");
                Entity.InteractionManager.SetInteractable(null);
            }
        }
        
    }

}

