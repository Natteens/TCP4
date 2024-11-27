using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public class CollectArea : MonoBehaviour
    {
        [SerializeField] private Production production;
        [SerializeField] private int amount;
        [SerializeField] private float timeToGive;
        private float currentTime;
        private bool isAbleToGive;
        private bool isGrown;
        private bool isMenuOpen;

        public void Start()
        {
            isMenuOpen = false;
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
            if (other.CompareTag("Player") && !isMenuOpen)
            {
                OpenProductionMenu();
            }
        }

        private void OpenProductionMenu()
        {
            // Lógica para abrir o menu de seleção de produção
            isMenuOpen = true;
            // Suponha que o menu chame o método SelectProduction quando uma produção é selecionada
        }

        public void SelectProduction(Production selectedProduction)
        {
            production = selectedProduction;
            isMenuOpen = false;
            StartCoroutine(GrowthCycle());
        }

        private IEnumerator GrowthCycle()
        {
            yield return new WaitForSeconds(production.timeToGrow);
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
