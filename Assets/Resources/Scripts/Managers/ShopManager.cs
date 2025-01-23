using ComponentUtils.ComponentUtils.Scripts;
using PlasticPipe.PlasticProtocol.Client;
using UnityEditor;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        private float stars = 0f;
        private int money = 0;

        public void IncreaseMoney(int value) { money += value;}
        public void DecreaseMoney(int value) { money -= value;}

        //Getters
        public float GetStars() => stars;
        public int GetMoney() => money;

    }

    
}