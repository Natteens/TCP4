using UnityEngine;

namespace Tcp4
{
    public class Customer : MonoBehaviour
    {
        public ReputationSystem reputationSystem;

        public void EvaluateOrder(Drink drink, float expectedQuality)
        {
            float actualQuality = drink.TotalQuality;

            if (actualQuality >= expectedQuality)
            {
                Debug.Log("Cliente satisfeito!");
                reputationSystem.AddReputation(0.1f);
            }
            else
            {
                Debug.Log("Cliente insatisfeito!");
                reputationSystem.RemoveReputation(0.1f);
            }
        }
    }
}