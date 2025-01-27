using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    [CreateAssetMenu(fileName = "NewDrink", menuName = "Menu/Drink")]
    public class Drink : ScriptableObject
    {
        public string drinkName;
        public Sprite drinkImage;
        public Ingredients[] requiredIngredients;
        public float preparationTime;
        
        public float baseMinPrice = 2f;
        public float baseMaxPrice = 5f;
    
        // calcula  o preco min e max com base na reputacao 
        public float minPrice => Mathf.Lerp(baseMinPrice, baseMinPrice * 4, reputationSystem.reputation / 5f);
        public float maxPrice => Mathf.Lerp(baseMaxPrice, baseMaxPrice * 4, reputationSystem.reputation / 5f);
        private float currentPrice; // preco ajustado pelo jogador

        public ReputationSystem reputationSystem;

        public float CurrentPrice
        {
            get { return currentPrice; }
            set { currentPrice = Mathf.Clamp(value, minPrice, maxPrice); }
        }

        // soma das qualidades dos ingredientes
        public float TotalQuality
        {
            get
            {
                float totalQuality = 0;
                foreach (var ingredient in requiredIngredients)
                {
                    totalQuality += ingredient.quality;
                }
                return totalQuality / requiredIngredients.Length;
            }
        }
    }
}