using UnityEngine;
using UnityEngine.UI;

namespace Tcp4
{
    public class ReputationDisplay : MonoBehaviour
    {
        public ReputationSystem reputationSystem;
        public Image[] stars;

        private void Update()
        {
            UpdateStars();
        }

        private void UpdateStars()
        {
            float rep = reputationSystem.reputation;

            for (int i = 0; i < stars.Length; i++)
            {
                if (i < Mathf.FloorToInt(rep))
                    stars[i].fillAmount = 1;
                else if (i == Mathf.FloorToInt(rep) && reputationSystem.reputationProgress == 0.5f) // meio cheio
                    stars[i].fillAmount = 0.5f;
                else
                    stars[i].fillAmount = 0;
            }
        }
    }
}