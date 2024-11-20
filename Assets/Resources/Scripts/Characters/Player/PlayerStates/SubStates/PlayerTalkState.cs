using Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SubStates
{
    public class PlayerTalkState : PlayerInteractableState
    {
        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            Debug.Log("Player entrou no estado de conversa. Iniciando diálogo...");

        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();

            Debug.Log("Player está conversando...");

            if (InputHandler.GetInteractInput())
            {
                Debug.Log("Saindo da conversa...");
                Entity.Machine.ChangeState("Idle", Entity);
            }
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
            Debug.Log("Player saiu do estado de conversa. Finalizando diálogo...");
        }
    }
}
