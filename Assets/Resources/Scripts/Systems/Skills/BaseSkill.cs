using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonReaper
{
    public abstract class BaseSkill : ScriptableObject, ISkill
    {
        public string skillName;
        public float cooldown;
        public bool isUsing = false;

        public virtual void ExecuteSkill(DynamicEntity player)
        {
            Debug.Log($"Executing skill: {skillName}");
        }

        public float GetCooldown() => cooldown;

    } 
}