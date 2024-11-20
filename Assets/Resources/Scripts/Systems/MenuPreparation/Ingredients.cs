using UnityEngine;

namespace Tcp4
{
    [CreateAssetMenu(fileName = "NewIngredient", menuName = "Menu/Ingredient")]
    public class Ingredients : ScriptableObject
    {
        public float quality;
        public IngredientType ingredientType;
    }

    public enum IngredientType
    {
        Cafe,
        Leite,
        Chocolate,
    }
}
