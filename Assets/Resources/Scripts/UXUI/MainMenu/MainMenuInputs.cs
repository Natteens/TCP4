using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuInputs : MonoBehaviour
{
    public PlayerInput playerInput;
    private bool canProceedToMenu = false;
    private bool isInputEnabled = false;

    private void OnEnable()
    {
        playerInput.actions["AnyKeyOrButton"].performed += OnAnyKeyOrButton;
        GameEventsMenu.OnEnableInput += EnableInput;
    }

    private void OnDisable()
    {
        playerInput.actions["AnyKeyOrButton"].performed -= OnAnyKeyOrButton;
        GameEventsMenu.OnEnableInput -= EnableInput;
    }

    public void OnAnyKeyOrButton(InputAction.CallbackContext ctx)
    {
        if (isInputEnabled)
        {
            canProceedToMenu = true;
            StartGame();
        }
    }

    private void StartGame()
    {
        Debug.Log("Jogo Iniciado!");
        GameEventsMenu.ClickToPlay();
    }

    private void Update()
    {
        if (!canProceedToMenu && isInputEnabled && Input.anyKeyDown)
        {
            canProceedToMenu = true;
            StartGame();
        }
    }

    private void EnableInput()
    {
        isInputEnabled = true;
    }

    
}


