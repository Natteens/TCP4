using ComponentUtils.ComponentUtils.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Tcp4
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject productionMenu;
        public Transform productionImagesParent;
        public Sprite sprProductionWait, sprRefinamentWait;
        public Sprite Ready;
        public GameObject pfImageToFill;

        public void ControlProductionMenu(bool _bool)
        { 
            productionMenu.SetActive(_bool);
        }

        public void SyncUIWithWorldObject(Transform worldObject, RectTransform uiElement)
        {
            // Converte a posi��o do objeto no mundo para a posi��o na tela
            Camera mainCamera = Camera.main;
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldObject.position);

            // Verifica se o objeto est� na frente da c�mera
            if (screenPosition.z > 0)
            {
                // Converte a posi��o da tela para a posi��o da UI
                Vector2 uiPosition;
                RectTransform canvasRect = uiElement.parent as RectTransform;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPosition,
                    mainCamera,
                    out uiPosition))
                {
                    // Clamping para garantir que a posi��o da UI esteja dentro dos limites da tela
                    uiPosition.x = Mathf.Clamp(uiPosition.x, 0, canvasRect.rect.width);
                    uiPosition.y = Mathf.Clamp(uiPosition.y, 0, canvasRect.rect.height);

                    // Define a posi��o do elemento da UI
                    uiElement.localPosition = uiPosition;
                    uiElement.gameObject.SetActive(true); // Garante que o elemento da UI est� ativo
                }
            }
            else
            {
                // Se o objeto estiver atr�s da c�mera, desativa o elemento da UI
                uiElement.gameObject.SetActive(false);
            }
        }




    }
}
