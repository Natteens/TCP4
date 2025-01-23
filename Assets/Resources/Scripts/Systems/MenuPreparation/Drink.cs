using UnityEngine;

namespace Tcp4
{
    [CreateAssetMenu(fileName = "NewDrink", menuName = "Menu/Drink")]
    public class Drink : BaseProduct
    {
        public string drinkName;
        public Sprite drinkImage;
        public float preparationTime;
    }
}
