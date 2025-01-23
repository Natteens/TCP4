using System;
using Tcp4.Resources.Scripts.Interfaces;
using Tcp4.Resources.Scripts.Types;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Systems.Interaction
{
    public class InteractableComponent : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractionType interactionType;
        public InteractionType InteractionKey => interactionType;

        // Eventos para a interação
        public event Action OnInteractionStart;
        public event Action OnInteractionExecute;
        public event Action OnInteractionEnd;

        public void StartInteraction()
        {
            OnInteractionStart?.Invoke();
            Debug.Log($"Interação {interactionType} iniciada.");
        }

        public void ExecuteInteraction()
        {
            OnInteractionExecute?.Invoke();
            Debug.Log($"Interação {interactionType} em andamento.");
        }

        public void EndInteraction()
        {
            OnInteractionEnd?.Invoke();
            Debug.Log($"Interação {interactionType} finalizada.");
        }
    }
}