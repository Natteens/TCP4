using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public interface ISkill
    {
        void ExecuteSkill(DynamicEntity player);
        float GetCooldown();
    }
}
