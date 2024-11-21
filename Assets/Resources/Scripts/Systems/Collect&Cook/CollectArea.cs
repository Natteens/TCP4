using UnityEngine;

namespace Tcp4
{
    public class CollectArea: MonoBehaviour
    {
        [SerializeField] private Ingredients ingredient;
        [SerializeField] private int amount;
        [SerializeField] private float timeToGive;
        private float currentTime;
        private bool isAbleToGive;

        public void Update()
        {
            if (currentTime > 0 && !isAbleToGive)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                isAbleToGive = true;
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && isAbleToGive)
            {
                Inventory playerInventory = other.GetComponent<Inventory>();
                playerInventory.AddIngredient(ingredient, amount);
                currentTime = timeToGive;
                isAbleToGive = false;
            }
        }
    }
}
