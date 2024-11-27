using Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates;
using Tcp4.Resources.Scripts.Systems.Interaction;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SubStates
{
    public class PlayerInteractState : PlayerGroundedState
    {
        private bool isInteractionComplete;
        
        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            InteractionEvents.OnInteractionEnded += CheckInteractionComplete;
            Movement(Vector2.zero);
            Entity.Rb.linearVelocity = Vector3.zero;
            InteractionHandler.TryInteract();
        }
        public override void DoFrameUpdateLogic()
        {
            Debug.Log("A interação foi completada? :" + isInteractionComplete);
            HandleStateTransitions();
        }
        private void CheckInteractionComplete()
        {
            isInteractionComplete = true;
        }
        protected override void HandleStateTransitions()
        {
            if (isInteractionComplete)
            {
                Entity.Machine.ChangeState(InputHandler.GetRawMovementDirection() != Vector3.zero ? "Move" : "Idle", Entity);
            }
        }
        
        public override void DoExitLogic()
        {
            base.DoExitLogic();
            InteractionEvents.OnInteractionEnded -= CheckInteractionComplete;
            isInteractionComplete = false;
            InteractionHandler.ClearCurrentTarget();
        }
    }
}
