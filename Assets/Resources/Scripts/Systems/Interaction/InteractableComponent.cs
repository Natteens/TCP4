using System;
using Tcp4.Resources.Scripts.Core;
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

        private bool isInteracting = false;

        public void StartInteraction()
        {
            if (isInteracting) return;
            isInteracting = true;
            OnInteractionStart?.Invoke();
        }

        public void ExecuteInteraction()
        {
            if (!isInteracting) return;
            OnInteractionExecute?.Invoke();
        }

        public void EndInteraction()
        {
            if (!isInteracting) return;
            isInteracting = false;
            OnInteractionEnd?.Invoke();
        }

        public bool IsInteracting => isInteracting;
    }
}
