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
            // Converte a posição do objeto no mundo para a posição na tela
            Camera mainCamera = Camera.main;
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldObject.position);

            // Verifica se o objeto está na frente da câmera
            if (screenPosition.z > 0)
            {
                // Converte a posição da tela para a posição da UI
                Vector2 uiPosition;
                RectTransform canvasRect = uiElement.parent as RectTransform;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPosition,
                    mainCamera,
                    out uiPosition))
                {
                    // Clamping para garantir que a posição da UI esteja dentro dos limites da tela
                    uiPosition.x = Mathf.Clamp(uiPosition.x, 0, canvasRect.rect.width);
                    uiPosition.y = Mathf.Clamp(uiPosition.y, 0, canvasRect.rect.height);

                    // Define a posição do elemento da UI
                    uiElement.localPosition = uiPosition;
                    uiElement.gameObject.SetActive(true); // Garante que o elemento da UI está ativo
                }
            }
            else
            {
                // Se o objeto estiver atrás da câmera, desativa o elemento da UI
                uiElement.gameObject.SetActive(false);
            }
        }




    }
}
