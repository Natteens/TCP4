using ComponentUtils.ComponentUtils.Scripts;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Systems.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [SerializeField] private InventoryData inventory;
    
        public bool AddItemToStorage(ItemData item, int amount = 1)
        {
            if (item == null) return false;
            inventory.AddItem(item, amount);
            return true;
        }
    
        public bool RemoveItemFromStorage(ItemData item, int amount = 1)
        {
            if (item == null) return false;
            return inventory.RemoveItem(item, amount);
        }
    
        public bool HasEnoughItems(ItemData item, int amount)
        {
            if (item == null) return false;
            return inventory.HasEnoughItems(item, amount);
        }
    
        public int GetItemCount(ItemData item)
        {
            if (item == null) return 0;
            return inventory.GetItemAmount(item);
        }
    }
}