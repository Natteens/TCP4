using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Tcp4.Assets.Resources.Scripts.Managers;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Systems.Clients
{
    public class Client: MonoBehaviour
    {
        public float stars;
        public float minimum;
        public string nameClient;
        public Drink wantedProduct;
        public Sprite spriteClient;
        public Sprite ui_wantedProductSprite;
        public Image ui_wantedProduct;
        public Image ui_timer;
        private float max_wait_time;
        private float wait_time;

        public void Setup(float _starts, float _minimum)
        {
            //setup basico
            stars = _starts;
            minimum = _minimum;
            spriteClient = GameAssets.Instance.clientSprites[Random.Range(0, GameAssets.Instance.clientSprites.Count)];
            nameClient = GameAssets.Instance.clientNames[Random.Range(0, GameAssets.Instance.clientSprites.Count)];
            max_wait_time = 120f - (stars * 2); 
            wait_time = max_wait_time;

            ChooseDrink();
        }

        public void Update()
        {

            if(wait_time > 0f) wait_time -= Time.deltaTime;
            else { NotDelivered(); }
        }

        public void Delivered()
        {
            ShopManager.Instance.IncreaseMoney(10); 
            ShopManager.Instance.IncreaseStar(0.1f + (stars / 10f));
        }

        public void NotDelivered()
        {
            ShopManager.Instance.DecreaseMoney(10);
             ShopManager.Instance.DecreaseStar(0.1f + (stars / 10f));
        }

        void ChooseDrink()
        {
            //Decidindo o pedido que eu quero!
            var rand = Random.Range(0, ShopManager.Instance.GetMenu().Count);

            Drink _drink = ShopManager.Instance.GetMenu()[rand];
            wantedProduct = _drink;
            ui_wantedProductSprite = wantedProduct.drinkImage;  
        }

    }
}