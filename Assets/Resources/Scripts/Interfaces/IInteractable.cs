using UnityEngine;

namespace CrimsonReaper
{
    public interface IInteractable
    {
        void OnPlayerEnter();
        void OnPlayerExit();
        void OnInteractStart(BaseEntity interactor);
        void OnInteractEnd(BaseEntity interactor);
    }
}
