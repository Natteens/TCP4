using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentUtils;

namespace CrimsonReaper
{
    public class Slime : DynamicEntity
    {
        private BasicEnemyIdleState idleState;
        private BasicEnemyPatrollState patrolState;

        private void Start()
        {
            RegisterBaseStates();
            machine.Initialize(idleState);
        }

        private void RegisterBaseStates()
        {
            idleState = new BasicEnemyIdleState();
            patrolState = new BasicEnemyPatrollState();

            machine.RegisterState("Idle", idleState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
            machine.RegisterState("Patrol", patrolState, this, abilitySet => abilitySet.GetAbilityValue(AbilityType.CanMove));
        }
    }
}
