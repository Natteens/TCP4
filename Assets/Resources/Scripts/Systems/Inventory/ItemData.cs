using Tcp4.Resources.Scripts.Types;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Systems.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private short id;
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private ItemType type;
    
        public short ID => id;
        public string Name => itemName;
        public string Description => description;
        public ItemType Type => type;
    
        private void OnValidate()
        {
            if (id < 0) id = 0;
        }
    }
}
