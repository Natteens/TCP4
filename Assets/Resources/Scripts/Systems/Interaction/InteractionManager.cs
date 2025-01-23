using System.Collections.Generic;
using GDX.Collections.Generic;
using Tcp4.Resources.Scripts.Interfaces;
using Tcp4.Resources.Scripts.Types;
using UnityEngine;
using UnityEngine.UI;

namespace Tcp4.Resources.Scripts.Systems.Interaction
{
    public abstract class InteractionManager : MonoBehaviour
    {
        public IInteractable CurrentInteractable;

        public void UpdateInteraction(InteractionType type)
        {
            UpdateInteractionUI(type);
        }
        
        protected abstract void UpdateInteractionUI(InteractionType type);

        public void SetInteractable(IInteractable interactable)
        {
            if (interactable != null)
            {

                if (CurrentInteractable != null)
                {
                    UnsubscribeEvents(CurrentInteractable);
                }
                CurrentInteractable = interactable;
                if (CurrentInteractable != null)
                {
                    SubscribeEvents(CurrentInteractable);
                    UpdateInteractionUI(CurrentInteractable.InteractionKey);
                }
            }
            else
            {
                CurrentInteractable = null;
            }
        }

        protected virtual void SubscribeEvents(IInteractable interactable)
        {
            interactable.OnInteractionStart += HandleInteractionStart;
            interactable.OnInteractionExecute += HandleInteractionExecute;
            interactable.OnInteractionEnd += HandleInteractionEnd;
        }

        protected virtual void UnsubscribeEvents(IInteractable interactable)
        {
            interactable.OnInteractionStart -= HandleInteractionStart;
            interactable.OnInteractionExecute -= HandleInteractionExecute;
            interactable.OnInteractionEnd -= HandleInteractionEnd;
        }

        protected abstract void HandleInteractionStart();
        protected abstract void HandleInteractionExecute();
        protected abstract void HandleInteractionEnd();
    }
}