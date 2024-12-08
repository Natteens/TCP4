using System.Collections;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Systems.Areas
{
    public class RefinamentArea : MonoBehaviour
    {
        [SerializeField] private RawProduct expectedProduct;
        [SerializeField] private float refinementTime = 5f;
        private bool isPlayerInArea = false;
        private bool isRefining = false;
        private Inventory playerInventory;

        private void Update()
        {
            if (isPlayerInArea && !isRefining && playerInventory != null && playerInventory.GetInventory().Contains(expectedProduct))
            {
                StartCoroutine(RefineProduct());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInArea = true;
                playerInventory = other.GetComponent<Inventory>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInArea = false;
                isRefining = false;
                playerInventory = null;
                StopAllCoroutines();
            }
        }

        private IEnumerator RefineProduct()
        {
            isRefining = true;
            yield return new WaitForSeconds(refinementTime);

            if (playerInventory != null && playerInventory.GetInventory().Contains(expectedProduct))
            {
                playerInventory.RefineProduct(expectedProduct);
            }

            isRefining = false;
        }
    }
}
