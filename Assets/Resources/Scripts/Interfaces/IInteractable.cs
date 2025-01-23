using System;
using Tcp4.Resources.Scripts.Types;

namespace Tcp4.Resources.Scripts.Interfaces
{
    public interface IInteractable
    {
        InteractionType InteractionKey { get; }
        event Action OnInteractionStart;
        event Action OnInteractionExecute;
        event Action OnInteractionEnd;
    
        void StartInteraction();
        void ExecuteInteraction();
        void EndInteraction();
    }
}
