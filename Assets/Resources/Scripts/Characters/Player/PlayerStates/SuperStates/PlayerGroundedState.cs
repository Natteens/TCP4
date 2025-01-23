using Tcp4.Resources.Scripts.FSM;
using Tcp4.Resources.Scripts.Systems.CollisionCasters;
using Tcp4.Resources.Scripts.Systems.Interaction;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates
{
    public class PlayerGroundedState : State<Player>
    {
        protected PlayerInputHandler InputHandler;
        private CollisionComponent _checker;
        protected InteractableHandler InteractionHandler;
        public override void Initialize(Player entity)
        {
            base.Initialize(entity);
            InputHandler = entity.ServiceLocator.GetService<PlayerInputHandler>();
            _checker = entity.Checker;
            InteractionHandler = entity.InteractableHandler;
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            HandleStateTransitions();
            CheckInteractable();
        }

        protected virtual void Movement(Vector3 input)
        {
            float speed = Entity.StatusComp.GetStatus(StatusType.Speed);
            Entity.Movement.Move(input, speed);
        }
        
        protected virtual void HandleStateTransitions()
        { 
            if (ShouldInteract())
            {
                Entity.Machine.ChangeState("Interact", Entity);
            } 
            else if (IsGrounded())
            {
                if (InputHandler.GetRawMovementDirection() != Vector3.zero && InputHandler.GetRunningInput())
                {
                    Entity.Machine.ChangeState("Run", Entity);
                }
                else if (InputHandler.GetRawMovementDirection() != Vector3.zero)
                {
                    Entity.Machine.ChangeState("Walk", Entity);
                }
                else
                {
                    Entity.Machine.ChangeState("Idle", Entity);
                }
            }
            
        }
        private bool ShouldInteract()
        {
            bool canInteract = CheckInteractable() && IsGrounded();
            return canInteract && InputHandler.GetInteractInput();
        }

        protected bool CheckInteractable()
        {
            if (_checker.IsColliding<EntityCollisionResult>("Interact", out var result))
            {
                InteractionHandler.OnCollisionDetected(result);
                return InteractionHandler.CurrentInteractable != null;
            }
        
            InteractionHandler.ClearCurrentTarget();
            return false;
        }

        private bool IsGrounded() => _checker.IsColliding<CollisionResult>("Ground", out var _);

    }
}

