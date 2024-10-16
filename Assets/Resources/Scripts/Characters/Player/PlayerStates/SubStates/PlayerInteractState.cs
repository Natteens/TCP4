using UnityEngine;

namespace Tcp4
{
    public class PlayerInteractState : PlayerGroundedState
    {
        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            currentInteractable?.OnInteractStart(entity);
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            if (currentInteractable == null)
            {
                entity.machine.ChangeState("Idle", entity);
            }
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
            currentInteractable?.OnInteractEnd(entity);
        }
    }

}
