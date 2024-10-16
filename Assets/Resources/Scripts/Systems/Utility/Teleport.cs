using System;
using UnityEngine;
using UnityEngine.Events;

namespace CrimsonReaper
{
    public class Teleport : StaticEntity, IInteractable
    {
        [SerializeField] private Transform teleportDestination;
        public GameObject ui;

        public void OnPlayerEnter()
        {
            ShowUI();
        }

        public void OnPlayerExit()
        {
            HideUI();
        }

        public void OnInteractStart(BaseEntity interactor)
        {
            TeleportToDestination(interactor);
        }

        public void OnInteractEnd(BaseEntity interactor)
        {
            // L�gica adicional ap�s o teleporte, se necess�rio
        }

        private void TeleportToDestination(BaseEntity interactor)
        {
            interactor.transform.position = teleportDestination.position;
            Debug.Log($"{interactor.name} foi teleportado.");
        }

        private void ShowUI()
        {
            Debug.Log("Jogador est� perto do teleporte. Mostrando UI.");
            ui.SetActive(true);
        }

        private void HideUI()
        {
            Debug.Log("Jogador se afastou do teleporte. Escondendo UI.");
            ui.SetActive(false);
        }
    }
}
