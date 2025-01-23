using ComponentUtils.ComponentUtils.Scripts;
using System;
using System.Collections.Generic;
using Tcp4.Assets.Resources.Scripts.Systems.Clients;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Tcp4.Assets.Resources.Scripts.Managers
{
    public class ClientManager : Singleton<ClientManager>
    {
        public event Action OnSpawnClient;
        public event Action<Client> OnClientSetup;
        [SerializeField] private List<Transform> clientSpots;
        [SerializeField] private GameObject prefab;
        [SerializeField] private List<GameObject> clients = new();

        [SerializeField] private bool canSpawn;
        [SerializeField] private float counter;
        [SerializeField] private float maxCounter;

        public void Start()
        {
            maxCounter = 2 - ShopManager.Instance.GetStars() / 2;
        }
        public void Spawn()
        {
            OnSpawnClient?.Invoke();

            float stars = UnityEngine.Random.Range(0f, ShopManager.Instance.GetStars());
            float minimum = UnityEngine.Random.Range(0.1f, ShopManager.Instance.GetStars() / 5f);

            GameObject _prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            Client prefabClient = _prefab.GetComponent<Client>();
            prefabClient.Setup(stars, minimum);

            OnClientSetup?.Invoke(prefabClient);

            clients.Add(_prefab);

            maxCounter = 2 - ShopManager.Instance.GetStars() / 2;
            counter = 0;
        }

        public void Update()
        {
            HandleLogicSpawn();
        }

        void HandleLogicSpawn()
        {
            if (!canSpawn) return;

            counter += Time.deltaTime;
            if (counter >= maxCounter) Spawn();
        }

        void OrganizeClients()
        {
            for(var i = 0; i < clients.Count; i++)
            {
                
            }

        }

        void DeleteClients()
        {
            foreach (GameObject c in clients)
            {
                Destroy(c);
            }

            clients.Clear();
        }

        public void StartSpawnClients() { canSpawn = true; Debug.Log("Posso spawnar clientes!"); }
        public void StopSpawnClients() { canSpawn = false; Debug.Log("NÃO posso spawnar clientes!"); counter = 0f; DeleteClients(); }
}
}