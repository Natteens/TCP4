using UnityEngine;

namespace Tcp4
{
    public class ReputationSystem : MonoBehaviour
    {
        [Range(0, 5)] public float reputation = 0f;
        public int reputationLevel => Mathf.FloorToInt(reputation);
        public float reputationProgress => Mathf.Round((reputation - Mathf.Floor(reputation)) * 2) / 2; // progresso para a prox estrela

        public void AddReputation(float value)
        {
            reputation = Mathf.Clamp(reputation + value, 0, 5);
            Debug.Log($"++Reputação: {reputation}");
        }

        public void RemoveReputation(float value)
        {
            reputation = Mathf.Clamp(reputation - value, 0, 5);
            Debug.Log($"--Reputação: {reputation}");
        }
    }
}