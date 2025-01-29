using ComponentUtils.ComponentUtils.Scripts;
using Tcp4.Assets.Resources.Scripts.Systems.Clients;
using Tcp4.Assets.Resources.Scripts.Systems.Collect_Cook;
using UnityEngine;
using UnityEngine.UI;

namespace Tcp4
{
    public class StorageManager : Singleton<StorageManager>
    {
        [SerializeField] private StorageArea currentStorage;
        public Inventory playerInventory;

        public void SetupCurrentStorage(StorageArea newStorage)
        {
            currentStorage = newStorage;
        }

         public void TransferItems()
        {
            var storageInventory = currentStorage.inventory;

            if(playerInventory == null || storageInventory == null) return;

            bool isAbleToTransfer = playerInventory.CountItem(currentStorage.item) > 0;

            if (isAbleToTransfer)
            {
                playerInventory.RemoveProduct(currentStorage.item, 1);
                storageInventory.AddProduct(currentStorage.item, 1);
            }
        }

    }
}
