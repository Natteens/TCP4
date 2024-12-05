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
                productInventory.RemoveAll(x => x.productID == product.productID);
                Despawn(product.model);
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
            var offset = productInventory.Count / 3.5f;
            GameObject instance = Instantiate(model, bagPoint);
            instance.transform.position += new Vector3(0, offset, 0);
            instanceInventory.Add(instance);
        }

        void Despawn(GameObject model)
        {
            if (instanceInventory.Count == 0) return;

            GameObject instanceToRemove = null;
            int index = 0;

            for (int i = 0; i < instanceInventory.Count; i++)
            {
                if (instanceInventory[i].GetComponent<MeshFilter>().mesh == model.GetComponent<MeshFilter>().mesh)
                {
                    instanceToRemove = instanceInventory[i];
                    index = i;
                }
            }

            if (instanceToRemove != null)
            {
                Destroy(instanceToRemove);
                instanceInventory.RemoveAt(index);
            }
        }
    }
}
