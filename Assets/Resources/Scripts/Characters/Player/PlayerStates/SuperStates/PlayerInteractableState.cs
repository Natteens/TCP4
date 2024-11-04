using Tcp4.Resources.Scripts.FSM;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates
{
    public class PlayerInteractableState : State<Player>
    {
        protected PlayerInputHandler InputHandler;
        protected CollisionComponent Checker;
        
        public override void Initialize(Player entity)
        {
            base.Initialize(entity);
            Checker = entity.Checker;
            InputHandler = entity.ServiceLocator.GetService<PlayerInputHandler>();
        }
        
        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
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
        }
    }
}
