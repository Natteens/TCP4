using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Systems.Collect_Cook
{
    public class StorageArea : MonoBehaviour
    {
        public Inventory storage;

        [SerializeField] private float timeToGive = 1f; // Tempo de espera para interagir novamente
        [SerializeField] private float interfaceDelay = 0.5f; // Tempo para exibir a interface
        private float currentTime;
        private bool isAbleToGive;
        private bool isInterfaceOpen;

        private void Start()
        {
            storage = GetComponent<Inventory>();
            currentTime = 0f;
            isAbleToGive = true;
            isInterfaceOpen = false;
        }

        private void Update()
        {
            if (currentTime > 0 && !isAbleToGive)
            {
                currentTime -= Time.deltaTime;
            }
            else if (currentTime <= 0)
            {
                isAbleToGive = true;
                currentTime = 0;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!isInterfaceOpen)
                {
                    StartCoroutine(OpenInterfaceAfterDelay());
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && isInterfaceOpen)
            {
                CloseInterface();
            }
        }

        private IEnumerator OpenInterfaceAfterDelay()
        {
            yield return new WaitForSeconds(interfaceDelay);

            if (!isInterfaceOpen)
            {
                UIManager.Instance.ControlStorageMenu(true);
                isInterfaceOpen = true;
            }
        }

        private void CloseInterface()
        {
            UIManager.Instance.ControlStorageMenu(false);
            isInterfaceOpen = false;
        }

        private void TransferItems(Collider player)
        {
            if (isAbleToGive)
            {
                Inventory playerInventory = player.GetComponent<Inventory>();
                List<BaseProduct> playerItems = playerInventory.GetInventory();

                if (playerItems.Count == 0) return;

                BaseProduct itemToTransfer = playerItems[^1];

                playerInventory.RemoveProduct(itemToTransfer, 1);
                storage.AddProduct(itemToTransfer, 1);

                isAbleToGive = false;
                currentTime = timeToGive;
            }
        }
    }
}
