using System.Collections.Generic;
using ComponentUtils.ComponentUtils.Scripts;
using PlasticPipe.PlasticProtocol.Client;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Tcp4.Assets.Resources.Scripts.Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        private float stars = 0f;
        private readonly float MaxStar = 1000f;

        private int money = 0;
        
        [SerializeField] private List<Drink> menu;

        public void IncreaseMoney(int value) { money += value;}
        public void IncreaseStar(float value) {stars += value; UpdateMenu();}
        public void DecreaseMoney(int value) { money -= value;}
        public void DecreaseStar(float value) {stars -= value; UpdateMenu();}

        public void AddNewDrink(Drink drink) {menu.Add(drink);}

        //Getters
        public float GetStars() => stars;
        public int GetMoney() => money;
        public List<Drink> GetMenu() => menu;

        void UpdateMenu()
        {
            //Adicionar ou remover itens do menu de acordo com as estrelas
            // No RefinamentManager tem uma lista de todos os Drinks do jogo e vc pode pegar um por ID
        }

    }

    
}