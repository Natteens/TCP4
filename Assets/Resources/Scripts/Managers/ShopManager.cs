using System;
using System.Collections;
using System.Collections.Generic;
using ComponentUtils.ComponentUtils.Scripts;
using PlasticPipe.PlasticProtocol.Client;
using Unity.Mathematics;
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

        public event Action OnChangeMoney, OnChangeStar;

        public GameObject porta;
        
        [SerializeField] private List<Drink> menu;

        public void IncreaseMoney(int value) 
        {
             money += value;
             OnChangeMoney.Invoke();
        }
        public void DecreaseMoney(int value) 
        { 
            money -= value; 
            OnChangeMoney.Invoke();
        }
        public void IncreaseStar(float value) 
        {
            stars += value; 
            OnChangeStar.Invoke();
            stars = Mathf.Clamp(stars, 0f, MaxStar);
        }
        public void DecreaseStar(float value) 
        {
            stars -= value;
            OnChangeStar.Invoke();
            stars = Mathf.Clamp(stars, 0f, MaxStar);
        }

        public void AddNewDrink(Drink drink) 
        {
            menu.Add(drink);
        }

        public void AbrirPorta()
        {
            porta.SetActive(false);
        }

        public void FecharPorta()
        {
            porta.SetActive(true);
        }

        //Getters
        public float GetStars() => stars;
        public float GetMaxStars() => MaxStar;
        public int GetMoney() => money;

        public List<Drink> GetMenu() => menu;


        void UpdateMenu()
        {
            //Adicionar ou remover itens do menu de acordo com as estrelas
            // No RefinamentManager tem uma lista de todos os Drinks do jogo e vc pode pegar um por ID
        }

        void Start()
        {
            StartCoroutine(InitialSetup());
        }

        IEnumerator InitialSetup()
        {
            yield return new WaitForSeconds(.2f);
            OnChangeMoney?.Invoke();
            OnChangeStar?.Invoke();
        }

    }

    
}