using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Systems.Collect_Cook
{
    public class StorageArea : MonoBehaviour
    {
        public Inventory inventory;
        public BaseProduct item;

        [SerializeField] private float interfaceDelay = 0.5f; // Tempo para exibir a interface

        private bool isInterfaceOpen;

        private void Start()
        {
            inventory = GetComponent<Inventory>();
            isInterfaceOpen = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!isInterfaceOpen)
                {
                    StartCoroutine(OpenInterfaceAfterDelay());
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && isInterfaceOpen)
            {
                CloseInterface();
            }
        }


        private IEnumerator OpenInterfaceAfterDelay()
        {
            yield return new WaitForSeconds(interfaceDelay);

            StorageManager.Instance.SetupCurrentStorage(this);

            if (!isInterfaceOpen)
            {
                UIManager.Instance.ControlStorageMenu(true);
                isInterfaceOpen = true;
            }
        }

        private void CloseInterface()
        {
            UIManager.Instance.ControlStorageMenu(false);
            StorageManager.Instance.SetupCurrentStorage(null);
            isInterfaceOpen = false;
        }

    }
}
