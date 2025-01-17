using ComponentUtils.ComponentUtils.Scripts;
using System.Collections;
using Tcp4.Resources.Scripts.Systems.DayNightCycle;
using UnityEditor;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Managers
{
    public class EventHandler : Singleton<EventHandler>
    {

        public ClientManager clientManager;
        public UIManager uiManager;
        public TimeManager timeManager;

        private void Start()
        {
            StartCoroutine(SubscribeEvents());
        }

        IEnumerator SubscribeEvents()
        {
            yield return new WaitForSeconds(1);
            timeManager.OnOpenCoffeeShop += uiManager.OpenShopNotification;
            timeManager.OnOpenCoffeeShop += clientManager.StartSpawnClients;

            timeManager.OnCloseCoffeeShop += uiManager.CloseShopNotification;
            timeManager.OnCloseCoffeeShop += clientManager.StopSpawnClients;

            clientManager.OnClientSetup += uiManager.NewClientNotification;
        }






    }
}