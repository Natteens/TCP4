using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Systems.Collect_Cook
{
    public class StorageArea : MonoBehaviour
    {
        public Inventory storage;

        [SerializeField] private float timeToGive;
        private float currentTime;
        private bool isAbleToGive;

        private void Start()
        {
            storage = GetComponent<Inventory>();
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
            if (other.CompareTag("Player") && isAbleToGive)
            {
                Inventory i = other.GetComponent<Inventory>();
                List<BaseProduct> playerInventory = i.GetInventory();

                if (playerInventory.Count == 0) return;

                BaseProduct ingredient = playerInventory[^1];

                i.RemoveProduct(ingredient, 1);
                storage.AddProduct(ingredient, 1);

                isAbleToGive = false;
                currentTime = timeToGive;
            }
        }
    }
}
