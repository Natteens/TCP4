using System.Collections;
using Tcp4.Assets.Resources.Scripts.Managers;
using UnityEngine;

namespace Tcp4
{
    public class CollectArea : MonoBehaviour
    {
        [SerializeField] private Production production;
        [SerializeField] private int amount;
        [SerializeField] private float timeToGive;
        [SerializeField] private Transform pointToSpawn;
        [SerializeField] private GameObject currentModel;

        private float currentTime;
        private bool isAbleToGive;
        private bool isGrown;
        private bool hasChoosedProduction;

        private void Start()
        {
            hasChoosedProduction = false;
            ProductionManager.Instance.OnChooseProduction += SelectProduction;
        }

        private void Update()
        {
            if (currentTime > 0 && !isAbleToGive)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                isAbleToGive = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!hasChoosedProduction)
                {
                    OpenProductionMenu();
                }
                else
                {
                    HarvestProduct(other);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CloseProductionMenu();
            }
        }

        private void OpenProductionMenu()
        {
            ProductionManager.Instance.Clean();
            ProductionManager.Instance.SetupNewReference(this);
            ProductionManager.Instance.ReloadCards();
            UIManager.Instance.ControlProductionMenu(true);
        }

        private void CloseProductionMenu()
        {
            ProductionManager.Instance.Clean();
            UIManager.Instance.ControlProductionMenu(false);
        }

        private void SelectProduction()
        {
            if (ProductionManager.Instance.GetCurrentReference() != this) return;

            production = ProductionManager.Instance.GetNewProduction();

            if (production == null) return;

            CloseProductionMenu();
            hasChoosedProduction = true;
            ProductionManager.Instance.OnChooseProduction -= SelectProduction;
            ProductionManager.Instance.Clean();
            StartCoroutine(GrowthCycle());
        }

        private IEnumerator GrowthCycle()
        {
            if (currentModel != null)
            {
                Destroy(currentModel);
            }

            foreach (var model in production.models)
            {
                if (currentModel != null)
                {
                    Destroy(currentModel);
                }
                currentModel = Instantiate(model, pointToSpawn.position, model.transform.rotation);
                yield return new WaitForSeconds(production.timeToGrow / production.models.Length);
            }

            isGrown = true;
        }

        private void HarvestProduct(Collider player)
        {
            if (isAbleToGive && isGrown)
            {
                Inventory playerInventory = player.GetComponent<Inventory>();
                playerInventory.AddProduct(production.product, amount);
                currentTime = timeToGive;
                isAbleToGive = false;
                isGrown = false;
                StartCoroutine(GrowthCycle());
            }
        }
    }
}
