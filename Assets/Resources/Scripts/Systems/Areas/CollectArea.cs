using System.Collections;
using System.Collections.Generic;
using Tcp4.Assets.Resources.Scripts.Managers;
using Tcp4.Assets.Resources.Scripts.UI;
using UnityEngine;

namespace Tcp4
{
    public class CollectArea : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Production production;
        [SerializeField] private float timeToGive;
        [SerializeField] private int amount;

        [Space(10)]

        [Header("View")]
        [SerializeField] private Transform pointToSpawn;
        [SerializeField] private GameObject currentModel;

        private ImageToFill timeImage;
        private Inventory playerInventory;
        private ObjectPool objectPools;
        private float currentTime;
        private bool isAbleToGive;
        private bool isGrown;
        private bool hasChoosedProduction;
        private const string PlayerTag = "Player";

        private void Start()
        {
            hasChoosedProduction = false;
            ProductionManager.Instance.OnChooseProduction += SelectProduction;
            var ui = UIManager.Instance;
            var obj = Instantiate(ui.pfImageToFill, ui.productionImagesParent);
            
            if (!obj.TryGetComponent<ImageToFill>(out timeImage)) Debug.LogError("DEU MERDA");
            if (timeImage.GetRectTransform() == null) Debug.LogError("DEU MERDA");

            ui.SyncUIWithWorldObject(pointToSpawn, timeImage.GetRectTransform());
            timeImage.ChangeSprite(null);
        }

        private void InitializeObjectPools()
        {
            objectPools = new ObjectPool(pointToSpawn);

            objectPools.AddPool(production.models);
            
        }

        private void Update()
        {
            if (!isAbleToGive)
            {
                currentTime -= Time.deltaTime;
                timeImage.UpdateFill(currentTime / timeToGive);

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    isAbleToGive = true;
                    timeImage.ChangeSprite(UIManager.Instance.Ready);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                playerInventory = other.GetComponent<Inventory>();
                if (!hasChoosedProduction)
                {
                    OpenProductionMenu();
                }
                else
                {
                    HarvestProduct();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                CloseProductionMenu();
                playerInventory = null;
            }
        }

        private void OpenProductionMenu()
        {
            var productionManager = ProductionManager.Instance;
            productionManager.Clean();
            productionManager.SetupNewReference(this);
            productionManager.ReloadCards();
            UIManager.Instance.ControlProductionMenu(true);
        }

        private void CloseProductionMenu()
        {
            ProductionManager.Instance.Clean();
            UIManager.Instance.ControlProductionMenu(false);
        }

        private void SelectProduction()
        {
            var productionManager = ProductionManager.Instance;
            if (productionManager.GetCurrentReference() != this) return;

            production = productionManager.GetNewProduction();

            if (production == null) return;

            timeImage.ChangeSprite(UIManager.Instance.sprProductionWait);
            CloseProductionMenu();
            hasChoosedProduction = true;
            productionManager.OnChooseProduction -= SelectProduction;
            productionManager.Clean();

            if (production.models.Length > 0)
            {
                InitializeObjectPools();
            }

            StartCoroutine(GrowthCycle());
        }

        private IEnumerator GrowthCycle()
        {
            if (currentModel != null)
            {
                objectPools.Return(currentModel);
            }

            var models = production.models;
            var timeToGrow = production.timeToGrow / models.Length;

            for (int i = 0; i < models.Length; i++)
            {
                if (currentModel != null)
                {
                    objectPools.Return(currentModel);
                }
                currentModel = objectPools.Get(models[i]);
                currentModel.transform.SetPositionAndRotation(pointToSpawn.position, models[i].transform.rotation);
                //currentModel.transform.localScale = models[i].transform.localScale;
                yield return new WaitForSeconds(timeToGrow);
            }

            isGrown = true;
        }

        private void HarvestProduct()
        {
            if (isAbleToGive && isGrown && playerInventory != null)
            {
                playerInventory.AddProduct(production.product, amount);
                currentTime = timeToGive;
                isAbleToGive = false;
                isGrown = false;
                StartCoroutine(GrowthCycle());
            }
        }
    }
}
