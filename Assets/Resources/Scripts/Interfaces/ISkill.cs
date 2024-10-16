using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonReaper
{
    public interface ISkill
    {
        void ExecuteSkill(DynamicEntity player);
        float GetCooldown();
    }
}
