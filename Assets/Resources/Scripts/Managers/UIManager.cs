using ComponentUtils.ComponentUtils.Scripts;
using Tcp4.Assets.Resources.Scripts.Systems.Clients;
using Tcp4.Assets.Resources.Scripts.Systems.Collect_Cook;
using UnityEngine;
using UnityEngine.UI;

namespace Tcp4
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject productionMenu;
        [SerializeField] private GameObject storageMenu;
        public Sprite sprProductionWait, sprRefinamentWait;
        public Sprite ready;
        public Sprite transparent;
        public GameObject pfImageToFill;
        public Canvas worldCanvas;

        public void ControlProductionMenu(bool _bool)
        { 
            productionMenu.SetActive(_bool);
        }


        public void ControlStorageMenu(bool _bool)
        {
            storageMenu.SetActive(_bool);
        }

        #region Notification System
        public void NewClientNotification(Client clientSettings)
        {
            Debug.Log("Novo cliente!");
            Debug.Log($"{clientSettings.nameClient}/ {clientSettings.stars}");
        }

        public void OpenShopNotification()
        {
            Debug.Log("Loja aberta!");
        }

        public void CloseShopNotification()
        {
            Debug.Log("Loja fechada!");
        }
        #endregion


        public void PlaceInWorld(Transform worldObject, RectTransform uiElement, bool isWorldCanvas = true)
        {
            if (!isWorldCanvas)
            {
                //Camera size
                Camera mainCamera = Camera.main;
                float camSize = mainCamera.orthographicSize;

                //Canvas size
                Vector2 canvasSize = new(worldCanvas.pixelRect.width, worldCanvas.pixelRect.height);

                //Formula: worldObjectPos * camSize * 2 / canvasSize

                float newX = worldObject.position.x * camSize * 2 / canvasSize.x;
                float newY = worldObject.position.y * camSize * 2 / canvasSize.y;
                float newZ = worldObject.position.z;

                Vector3 newPos = new(newX, newY, newZ);
                uiElement.position = newPos;
            }
            else
            {
                Vector3 offset = new(0f, 2f, 0f);
                uiElement.position = worldObject.position + offset;
            }
            

        }

 



    }
}
