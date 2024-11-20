using System.Collections;
using Tcp4.Assets.Resources.Scripts.Characters.Player.PlayerStates.SuperStates;
using Tcp4.Assets.Resources.Scripts.Core;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Characters.Player.PlayerStates.SubStates
{
    public class PlayerRunningState : PlayerGroundedState
    {
        private float RunningMultiplier = 1.5f;
        protected override void ConfigureAnimation()
        {
            StateAnimation = new AnimationData(
                stateName: "PlayerRun",
                transitionDuration: 0
            );
        }

        public override void DoPhysicsLogic()
        {
            base.DoPhysicsLogic();
            Movement(InputHandler.GetRawMovementDirection());
        }

        protected override void Movement(Vector3 input)
        {
            float speed = Entity.StatusComp.GetStatus(StatusType.Speed) * RunningMultiplier;
            Entity.Movement.Move(input, speed);
        }
    }
}