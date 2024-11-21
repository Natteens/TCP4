using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<Ingredients> ingredientInventory;
        [SerializeField] private List<GameObject> instanceInventory; 
        [SerializeField] private Transform bagPoint;
        [SerializeField] private GameObject pfDebugItem; //tirar isso dps e substituir dentro dos proprios ingredientes os modelos

        public List<Ingredients> GetInventory() => ingredientInventory;

        public void AddIngredient(Ingredients ingredient, int amount) 
        {
            CheckError(
                () => amount <= 0,
                () => ingredient == null
            );

            for (int i = 0; i < amount; i++)
            {
                ingredientInventory.Add(ingredient);
                Spawn();
            }
        }


        public void RemoveIngredient(Ingredients ingredient, int amount)
        {
            CheckError(
                () => amount <= 0,
                () => ingredient == null
            );

            for (int i = 0; i < amount; i++)
            {
                ingredientInventory.Remove(ingredient);
                Despawn();
            }
        }

        void CheckError(params Func<bool>[] conditions)
        {
            foreach (var condition in conditions)
            {
                if (condition())
                {
                    Debug.LogError($"Erro encontrado na condição {condition}");
                    return;
                }
            }
        }

        void Spawn()
        {
            var offset = ingredientInventory.Count / 10f;
            GameObject instance = Instantiate(pfDebugItem, bagPoint);
            Vector3 newPosition = new(
                instance.transform.position.x,
                instance.transform.position.y + offset,
                instance.transform.position.z
            );
            instance.transform.position = newPosition;

            instanceInventory.Add(instance);

        }

        void Despawn()
        {
            int INDEX_TO_REMOVE = instanceInventory.Count - 1;
            Destroy(instanceInventory[INDEX_TO_REMOVE]);
            instanceInventory.RemoveAt(INDEX_TO_REMOVE); //removendo qualquer porra por enquanto pq nao ta pegando os modelo bonitinho
        }

    }
}