using UnityEngine;
using System.Collections.Generic;
using GDX.Collections.Generic;
namespace Tcp4.Resources.Scripts.Systems.Inventory
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Storage")]
    public class InventoryData : ScriptableObject
    {
        private SerializableDictionary<short, int> items = new SerializableDictionary<short, int>();
    
        public void AddItem(ItemData item, int amount = 1)
        {
            if (items.ContainsKey(item.ID))
                items[item.ID] += amount;
            else
                items[item.ID] = amount;
        }
    
        public bool RemoveItem(ItemData item, int amount = 1)
        {
            if (!items.ContainsKey(item.ID) || items[item.ID] < amount)
                return false;
            
            items[item.ID] -= amount;
        
            if (items[item.ID] <= 0)
                items.Remove(item.ID);
            
            return true;
        }
    
        public int GetItemAmount(ItemData item)
        {
            return items.ContainsKey(item.ID) ? items[item.ID] : 0;
        }
    
        public bool HasEnoughItems(ItemData item, int amount)
        {
            return GetItemAmount(item) >= amount;
        }
    
        public void Clear()
        {
            items.Clear();
        }
    }
}