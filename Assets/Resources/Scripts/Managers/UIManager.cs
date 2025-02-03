using System.Collections.Generic;
using System.Linq;
using ComponentUtils.ComponentUtils.Scripts;
using GDX.Collections.Generic;
using Tcp4.Assets.Resources.Scripts.Managers;
using Tcp4.Assets.Resources.Scripts.Systems.Clients;
using Tcp4.Assets.Resources.Scripts.Systems.Collect_Cook;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;

namespace Tcp4
{
    public class UIManager : Singleton<UIManager>
    {
        #region UI Elements

        [Header("Menus")]
        [SerializeField] private GameObject productionMenu;
        [SerializeField] private GameObject storageMenu;
        [SerializeField] private GameObject configMenu;

        [Header("Sprites")]
        public Sprite sprProductionWait;
        public Sprite sprRefinamentWait;
        public Sprite ready;
        public Sprite transparent;

        [Header("Prefabs")]
        public GameObject pfImageToFill;
        public GameObject pfSlotStorage;

        [Header("UI Containers")]
        public Transform slotHolder;
        public Canvas worldCanvas;

        [Header("UI Animations")]
        public AnimationExecute money;
        public AnimationExecute stars;

        #endregion

        #region UI Text & Images

        [Header("UI Text")]
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI nameStorage;
        public TextMeshProUGUI amountStorage;

        [Header("UI Images")]
        public Image starImage;

        #endregion

        #region Storage Management

        private List<GameObject> slotInstances = new();

        public void ControlStorageMenu(bool isActive) => storageMenu.SetActive(isActive);

        public void QuitApplication() => UnityEngine.Application.Quit();

        public void ControlConfigMenu()
        {
            if (configMenu.activeSelf)
            {
                configMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                configMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }

        public void CleanStorageSlots()
        {
            if (slotInstances == null || slotInstances.Count == 0) return;

            foreach (var go in slotInstances)
            {
                Destroy(go);
            }
            slotInstances.Clear();
        }

        public void UpdateStorageView()
        {
            StorageArea storage = StorageManager.Instance.GetStorageArea();
            if (storage == null) return;

            Inventory inventory = storage.inventory;

            nameStorage.text = storage.item.productName;
            amountStorage.text = $"{inventory.CountItem(storage.item)} / {inventory.GetLimit()}";

            CleanStorageSlots();

            foreach (BaseProduct _ in inventory.GetInventory())
            {
                GameObject go = Instantiate(pfSlotStorage, slotHolder);
                slotInstances.Add(go);
                go.GetComponent<DataStorageSlot>().Setup(storage.item.productImage, 1);
            }
        }

        #endregion

        #region Notifications

        public void NewClientNotification(Client clientSettings)
        {
            Debug.Log($"Novo cliente: {clientSettings.nameClient}, Estrelas: {clientSettings.stars}");
        }

        public void OpenShopNotification() => Debug.Log("Loja aberta!");
        public void CloseShopNotification() => Debug.Log("Loja fechada!");

        #endregion

        #region Shop Management

        public void UpdateMoney()
        {
            moneyText.text = ShopManager.Instance.GetMoney().ToString();
            money.ExecuteAnimation("pop");
        }

        public void UpdateStars()
        {
            starImage.fillAmount = ShopManager.Instance.GetStars() / ShopManager.Instance.GetMaxStars();
            stars.ExecuteAnimation("pop");
        }

        #endregion

        #region Utility

        public void ControlProductionMenu(bool isActive) => productionMenu.SetActive(isActive);

        public void PlaceInWorld(Transform worldObject, RectTransform uiElement, bool isWorldCanvas = true)
        {
            if (!isWorldCanvas)
            {
                Camera mainCamera = Camera.main;
                float camSize = mainCamera.orthographicSize;
                Vector2 canvasSize = new(worldCanvas.pixelRect.width, worldCanvas.pixelRect.height);

                float newX = worldObject.position.x * camSize * 2 / canvasSize.x;
                float newY = worldObject.position.y * camSize * 2 / canvasSize.y;
                float newZ = worldObject.position.z;

                uiElement.position = new Vector3(newX, newY, newZ);
            }
            else
            {
                uiElement.position = worldObject.position + new Vector3(0f, 2f, 0f);
            }
        }

        #endregion
    }
}
