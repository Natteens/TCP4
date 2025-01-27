using UnityEngine;
using UnityEngine.UI;

namespace Tcp4
{
    public class PriceManager : MonoBehaviour
    {
        public Slider priceSlider; // onde o jogador altera o preco do produto
        public Text priceText;
        public Drink drink;
        public ReputationSystem reputationSystem;

        private void Start()
        {
            priceSlider.minValue = drink.minPrice;
            priceSlider.maxValue = drink.maxPrice;
            priceSlider.value = (drink.minPrice + drink.maxPrice) / 2f;

            UpdatePriceText();
        }

        private void Update()
        {
            drink.CurrentPrice = priceSlider.value;

            UpdatePriceText();
        }

        private void UpdatePriceText()
        {
            priceText.text = $" R${drink.CurrentPrice} - Qualidade: {drink.TotalQuality}";
        }
    }
}