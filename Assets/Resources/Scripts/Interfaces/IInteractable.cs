using UnityEngine;

namespace Tcp4
{
    public interface IInteractable
    {
        void OnPlayerEnter();
        void OnPlayerExit();
        void OnInteractStart(BaseEntity interactor);
        void OnInteractEnd(BaseEntity interactor);
    }
}
