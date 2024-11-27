using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<BaseProduct> productInventory = new List<BaseProduct>();
        [SerializeField] private List<GameObject> instanceInventory = new List<GameObject>();
        [SerializeField] private Transform bagPoint;
        [SerializeField] private GameObject pfDebugItem; //tirar isso dps e substituir dentro dos proprios ingredientes os modelos

        public List<BaseProduct> GetInventory() => productInventory;

        public void AddProduct(BaseProduct product, int amount)
        {
            if (amount <= 0 || product == null)
            {
                Debug.LogError("Erro: quantidade inválida ou produto nulo.");
                return;
            }

            for (int i = 0; i < amount; i++)
            {
                productInventory.Add(product);
                Spawn();
            }
        }

        public void RemoveProduct(BaseProduct product, int amount)
        {
            if (amount <= 0 || product == null)
            {
                Debug.LogError("Erro: quantidade inválida ou produto nulo.");
                return;
            }

            for (int i = 0; i < amount; i++)
            {
                productInventory.RemoveAll(x => x.productID == product.productID);
                Despawn();
            }
        }

        void Spawn()
        {
            var offset = productInventory.Count / 10f;
            GameObject instance = Instantiate(pfDebugItem, bagPoint);
            instance.transform.position += new Vector3(0, offset, 0);
            instanceInventory.Add(instance);
        }

        void Despawn()
        {
            if (instanceInventory.Count == 0) return;

            int indexToRemove = instanceInventory.Count - 1;
            Destroy(instanceInventory[indexToRemove]);
            instanceInventory.RemoveAt(indexToRemove);
        }
    }
}
