using CrimsonReaper.Resources.Scripts.Core;
using Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SubStates
{
    public class PlayerIdleState : PlayerGroundedState
    {
        protected override void ConfigureAnimation()
        {
            StateAnimation = new AnimationData(
                stateName: "PlayerIdle",
                transitionDuration: 0
            );
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            Movement(Vector3.zero);
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }
    }
}
