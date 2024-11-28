using System.Collections;
using System.Collections.Generic;
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

        public void Start()
        {
            hasChoosedProduction = false;
            ProductionManager.Instance.OnChooseProduction += SelectProduction;
        }

        public void Update()
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

        public void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && !hasChoosedProduction)
            {
                OpenProductionMenu();
            }
            else if (other.CompareTag("Player"))
            {
                HarvestProduct(other);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CloseProductionMenu();
            }
        }

        private void OpenProductionMenu()
        {
            UIManager.Instance.ControlProductionMenu(true);
        }

        private void CloseProductionMenu()
        {
            UIManager.Instance.ControlProductionMenu(false);
        }

        public void SelectProduction()
        {
            hasChoosedProduction = true;
            production = ProductionManager.Instance.GetNewProduction();
            ProductionManager.Instance.OnChooseProduction -= SelectProduction;
            CloseProductionMenu();
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


        public void HarvestProduct(Collider player)
        {
            if (player.CompareTag("Player") && isAbleToGive && isGrown)
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