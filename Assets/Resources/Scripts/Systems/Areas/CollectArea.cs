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
            var obj = Instantiate(ui.pfImageToFill, ui.worldCanvas.gameObject.transform);
            timeImage = obj.GetComponent<ImageToFill>();
            ui.PlaceInWorld(pointToSpawn, timeImage.GetRectTransform());
        }

        private void InitializeObjectPools()
        {
            objectPools = new ObjectPool(pointToSpawn);
            objectPools.AddPool(production.models);
        }

        private void Update()
        {
            SpritesLogic();
            //UpdateCurrentTime();
        }

        private void UpdateCurrentTime()
        {
            if (production != null && !isGrown)
            {
                currentTime = Mathf.Clamp(currentTime, 0, production.timeToGrow);
                timeImage.UpdateFill(currentTime);
            }
        }

        private void SpritesLogic()
        {
            if (production == null)
            {
                timeImage.ChangeSprite(UIManager.Instance.transparent);
                return;
            }

            if (!isAbleToGive && currentTime < production.timeToGrow)
            {
                timeImage.ChangeSprite(UIManager.Instance.sprProductionWait);
            }
            else if (isAbleToGive)
            {
                timeImage.ChangeSprite(UIManager.Instance.ready);
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

            currentTime = 0;
            timeImage.SetupMaxTime(production.timeToGrow);
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

            var models = production.models;
            var timeToGrow = production.timeToGrow;
            int modelIndex = 0;

            while (modelIndex < models.Length)
            {
                if (currentModel != null)
                {
                    objectPools.Return(currentModel);
                }

                currentModel = objectPools.Get(models[modelIndex]);
                currentModel.transform.SetPositionAndRotation(pointToSpawn.position, models[modelIndex].transform.rotation);
                //Debug.Log($"Modelo atual: {currentModel.name} / Rotacao: {currentModel.transform.rotation} / Index: {modelIndex}");

                float modelGrowTime = timeToGrow / models.Length;
                float elapsedTime = 0;

                while (elapsedTime < modelGrowTime)
                {
                    elapsedTime += Time.deltaTime;
                    currentTime += Time.deltaTime;
                    UpdateCurrentTime();
                    yield return null;
                }

                modelIndex++;
            }

            isGrown = true;
            isAbleToGive = true;
        }

        private void HarvestProduct()
        {
            if (isAbleToGive && isGrown && playerInventory != null)
            {
                playerInventory.AddProduct(production.product, amount);
                currentTime = 0;
                isAbleToGive = false;
                isGrown = false;
                StartCoroutine(GrowthCycle());
            }
        }
    }
}
