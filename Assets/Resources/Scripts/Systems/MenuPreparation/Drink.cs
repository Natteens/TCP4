using UnityEngine;

namespace Tcp4
{
    [CreateAssetMenu(fileName = "NewDrink", menuName = "Menu/Drink")]
    public class Drink : ScriptableObject
    {
        public string drinkName;
        public Ingredients[] requiredIngredients;
        public float quality;
        public float preparationTime;
    }
}
