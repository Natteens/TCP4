using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonReaper
{
    public class SkillComponent : Singleton<SkillComponent>
    {
        [SerializeField] private BaseSkill baseSkill;
        public BaseSkill equippedSkill { get; private set; }
        public bool isOnCooldown = false;


        private void Start()
        {
            equippedSkill = baseSkill;
        }

        public void EquipSkill(BaseSkill newSkill)
        {
            equippedSkill = newSkill;
        }

        public void UnequipSkill()
        {
            equippedSkill = baseSkill;
        }

        public void ExecuteSkill(Player player)
        {
            if (!isOnCooldown)
            {
                equippedSkill.ExecuteSkill(player);
                StartCoroutine(CooldownRoutine(equippedSkill.GetCooldown()));
                Debug.Log($"executando skill {baseSkill.skillName} ");
            }
        }

        private IEnumerator CooldownRoutine(float cooldown)
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(cooldown);
            isOnCooldown = false;
        }
    }

}