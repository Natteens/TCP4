using Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow.CowStates.SuperStates;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Characters.Animals.Cow.CowStates.SubStates
{
    public class CowTalkState : CowInteractableState
    {
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
            Debug.Log("Finalizando interação com a vaca.");
        }
    }
}
