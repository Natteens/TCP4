using ComponentUtils.ComponentUtils.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4
{
    public class RefinamentManager : Singleton<RefinamentManager>
    {
        [SerializeField] private List<RefinementRecipe> recipes;

        public BaseProduct Refine(BaseProduct inputProduct)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.inputProduct == inputProduct)
                {
                    return recipe.outputProduct;
                }
            }

            Debug.LogError("Nenhuma receita de refinamento encontrada para o produto: " + inputProduct.productName);
            return null;
        }
    }

    [System.Serializable]
    public class RefinementRecipe
    {
        public BaseProduct inputProduct;
        public BaseProduct outputProduct;
    }
}
