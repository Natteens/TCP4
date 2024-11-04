using GDX.Collections.Generic;
using UnityEngine.UI;
using Tcp4.Resources.Scripts.Types;
using UnityEngine;
using Tcp4.Resources.Scripts.Systems.Utility;

namespace Tcp4.Resources.Scripts.Systems.Interaction
{
    public class PlayerInteractionManager : InteractionManager
    {
        [SerializeField] protected SerializableDictionary<InteractionType, Sprite> interactionSprites;
        [SerializeField] private Image interactionIcon;
        
        [Header("Events")]
        [SerializeField] private GameEvent onInteractionEnter;    // Quando detecta uma interação
        [SerializeField] private GameEvent onInteractionExit;     // Quando sai da área de interação
        [SerializeField] private GameEvent onInteractionExecute;  // Quando executa a interação

        [Header("Default Settings")]
        [SerializeField] private Sprite defaultIcon;
        
        protected override void UpdateInteractionUI(InteractionType type)
        {
            SwitchIconsUI(type);
        }
        
        public void SwitchUI()
        {
            SwitchIconsUI(CurrentInteractable.InteractionKey);
        }
        private void SwitchIconsUI(InteractionType type)
        {
            if (interactionSprites.TryGetValue(type, out var sprite))
            {
                interactionIcon.sprite = sprite;
                interactionIcon.enabled = true;
            }
            else
            {
                SetDefaultInteractionIcon();
            }
        }

        private void SetDefaultInteractionIcon()
        {
            if (defaultIcon != null)
            {
                interactionIcon.sprite = defaultIcon;
            }
            onInteractionExit?.Raise();
        }

        protected override void HandleInteractionStart() 
        { 
            onInteractionExecute?.Raise();
        }

        protected override void HandleInteractionExecute()
        {
            onInteractionEnter?.Raise(); 
        }
        
        protected override void HandleInteractionEnd()
        {
            SetDefaultInteractionIcon();
        }
    }
}