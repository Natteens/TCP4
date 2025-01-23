using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Tcp4.Assets.Resources.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tcp4.Assets.Resources.Scripts.Systems.Clients
{
    public class Client: MonoBehaviour
    {
        public string ID;
        public float stars;
        public float minimum;
        public string nameClient;
        public  TextMeshProUGUI nameTmp;
        public Drink wantedProduct;
        public Sprite spriteClient;
        public UnityEngine.UI.Image ui_wantedProduct;
        public UnityEngine.UI.Image ui_timer;
        private float max_wait_time;
        private float wait_time;

        public void Setup(float _starts, float _minimum)
        {
            //setup basico
            stars = _starts;
            minimum = _minimum;
            spriteClient = GameAssets.Instance.clientSprites[Random.Range(0, GameAssets.Instance.clientSprites.Count)];
            nameClient = GameAssets.Instance.clientNames[Random.Range(0, GameAssets.Instance.clientSprites.Count)];
            nameTmp.text = nameClient;
            max_wait_time = 15f - (stars * 2); 
            wait_time = max_wait_time;
            ui_timer.fillAmount = wait_time / max_wait_time;
            ID = GameAssets.GenerateID(5);

            ChooseDrink();
        }

        public void Update()
        {
            ui_timer.fillAmount = wait_time / max_wait_time;

            if(wait_time > 0f) wait_time -= Time.deltaTime;
            else { NotDelivered(); }
        }

        public void Delivered()
        {
            ShopManager.Instance.IncreaseMoney(10); 
            ShopManager.Instance.IncreaseStar(0.1f + (stars / 10f));
            ClientManager.Instance.DeleteSpecificClient(this);
        }

        public void NotDelivered()
        {
            ShopManager.Instance.DecreaseMoney(10);
            ShopManager.Instance.DecreaseStar(0.1f + (stars / 10f));
            ClientManager.Instance.DeleteSpecificClient(this);
        }

        public void ChooseDrink()
        {
            //Decidindo o pedido que eu quero!
            var rand = Random.Range(0, ShopManager.Instance.GetMenu().Count - 1);

            Drink _drink = ShopManager.Instance.GetMenu()[rand];
            wantedProduct = _drink;
            ui_wantedProduct.sprite = wantedProduct.drinkImage; 
        }

    }
}