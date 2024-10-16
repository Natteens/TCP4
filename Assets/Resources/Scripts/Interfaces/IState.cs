using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public interface IState
    {
        void Initialize(DynamicEntity entity);
        void DoEnterLogic();
        void DoExitLogic();
        void DoFrameUpdateLogic();
        void DoPhysicsLogic();
        void DoChecks();
        void ResetValues();
    }
}
