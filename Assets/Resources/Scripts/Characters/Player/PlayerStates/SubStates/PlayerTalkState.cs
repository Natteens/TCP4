using Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SuperStates;
using Tcp4.Resources.Scripts.FSM;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Characters.Player.PlayerStates.SubStates
{
    public class PlayerTalkState : PlayerInteractableState
    {
        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            Debug.Log("Player entrou no estado de conversa. Iniciando diálogo...");

            // Aqui você poderia exibir uma interface de diálogo ou iniciar uma sequência de fala
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();

            // Adicione uma lógica simples para simular uma interação de fala
            Debug.Log("Player está conversando...");
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
            Debug.Log("Player saiu do estado de conversa. Finalizando diálogo...");
        }
    }
}