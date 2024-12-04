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

            for(int i = 0; i < instanceInventory.Count; i++)
            {
                if (instanceInventory[i].GetComponent<MeshFilter>().mesh == model.GetComponent<MeshFilter>().mesh)
                {
                    instanceToRemove = instanceInventory[i];
                    index = i;
                }
                    
            }
               
            if(instanceToRemove != null)
            {
                Destroy(instanceToRemove);
                instanceInventory.RemoveAt(index);
            }
     
           
        }
    }
}
