using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<BaseProduct> productInventory = new();
        [SerializeField] private List<GameObject> instanceInventory = new();
        [SerializeField] private Transform bagPoint;

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
                Spawn(product.model);
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
                BaseProduct itemToRemove = productInventory.Find(x => x.productID == product.productID);
                if (itemToRemove != null)
                {
                    productInventory.Remove(itemToRemove);
                    Despawn(product.model);
                }
            }
        }

        public void RefineProduct(BaseProduct product)
        {
            if (product == null)
            {
                Debug.LogError("Erro: produto nulo.");
                return;
            }

            BaseProduct refinedProduct = RefinamentManager.Instance.Refine(product);

            if (refinedProduct != null)
            {
                RemoveProduct(product, 1);
                AddProduct(refinedProduct, 1);
            }
        }

        void Spawn(GameObject model)
        {
            GameObject instance = Instantiate(model, bagPoint);
            instanceInventory.Add(instance);
            ReorganizeInventory();
        }

        void Despawn(GameObject model)
        {
            if (instanceInventory.Count == 0) return;

            GameObject instanceToRemove = null;
            int index = -1;

            for (int i = 0; i < instanceInventory.Count; i++)
            {
                if (instanceInventory[i].GetComponent<MeshFilter>().sharedMesh == model.GetComponent<MeshFilter>().sharedMesh)
                {
                    instanceToRemove = instanceInventory[i];
                    index = i;
                    break; // Encontramos a instância, podemos sair do loop
                }
            }

            if (instanceToRemove != null && index != -1)
            {
                Destroy(instanceToRemove);
                instanceInventory.RemoveAt(index);
                ReorganizeInventory();
            }
        }

        void ReorganizeInventory()
        {
            for (int i = 0; i < instanceInventory.Count; i++)
            {
                var offset = i / 3.5f;
                instanceInventory[i].transform.position = bagPoint.position + new Vector3(0, offset, 0);
            }
        }
        
    }
}
