using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public class DebugComponent : MonoBehaviour
    {
        private DynamicEntity dynamicEntity;
        private StaticEntity staticEntity;

        [Header("Debug GUI Settings")]
        public bool isPlayer = false; // Marca se esta entidade é o jogador
        public Vector2 guiOffset = new Vector2(0, 1.5f); // Offset para posicionar a GUI
        public int textSize = 20; // Tamanho do texto da GUI
        public Color textColor = Color.white; // Cor do texto

        private void Start()
        {
            TryGetComponent<DynamicEntity>(out dynamicEntity);
            TryGetComponent<StaticEntity>(out staticEntity);
        }

        private void OnGUI()
        {
            GUI.skin.label.fontSize = textSize; // Ajusta o tamanho da fonte

            if (dynamicEntity != null)
            {
                DisplayDynamicEntityInfo(dynamicEntity);
            }
            else if (staticEntity != null)
            {
                DisplayStaticEntityInfo(staticEntity);
            }
            else
            {
                Debug.LogWarning("Nenhuma entidade válida encontrada.");
            }
        }

        private void DisplayDynamicEntityInfo(DynamicEntity entity)
        {
            // Se é o jogador, exibe informações no canto superior esquerdo
            if (isPlayer)
            {
                GUI.color = textColor; // Ajusta a cor do texto

                // Certifique-se de que a altura da caixa da GUI seja suficiente para o tamanho do texto
                GUI.Label(new Rect(10, 10, 300, textSize + 5), $"State: {entity.machine.CurrentState.GetType().Name}");
                GUI.Label(new Rect(10, 10 + textSize + 5, 300, textSize + 5), $"FPS: {Mathf.Round(1 / Time.deltaTime)}");

                // Acessando variáveis e métodos públicos dinamicamente
                foreach (var field in entity.GetType().GetFields())
                {
                    GUI.Label(new Rect(10, 30 + (textSize + 5) * field.MetadataToken, 300, textSize + 5), $"{field.Name}: {field.GetValue(entity)}");
                }
            }
            else
            {
                // Para outras entidades, a GUI aparece acima da entidade
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(entity.transform.position + new Vector3(guiOffset.x, guiOffset.y, 0));
                GUI.color = textColor;

                // Ajustando a altura do Rect para o tamanho correto do texto
                GUI.Label(new Rect(screenPosition.x - 50, Screen.height - screenPosition.y - 50, 300, textSize + 5), $"State: {entity.machine.CurrentState.GetType().Name}");
                // Adicione mais informações conforme necessário
            }
        }

        private void DisplayStaticEntityInfo(StaticEntity entity)
        {
            // Exibe informações do StaticEntity acima da entidade
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(entity.transform.position + new Vector3(guiOffset.x, guiOffset.y, 0));
            GUI.color = textColor;

            // Ajustando a altura do Rect para o tamanho correto do texto
            GUI.Label(new Rect(screenPosition.x - 50, Screen.height - screenPosition.y - 50, 300, textSize + 5), $"Static Entity: {entity.name}");
            // Adicione mais informações conforme necessário
        }
    }
}
