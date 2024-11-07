using System;
using Tcp4.Resources.Scripts.Characters.Player;
using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{

    [Header("Output")]
    public PlayerInputHandler inputs;

    public void OnEnable()
    {
        inputs = FindObjectOfType<PlayerInputHandler>();
    }

    public void VirtualMoveInput(Vector3 virtualMoveDirection)
    {
        inputs.OnGetRawMovement(virtualMoveDirection);
    }
    
    public void VirtualInteractInput(bool virtualInputState)
    {
        inputs.OnInteract(virtualInputState);
    }
    
    public void VirtualSprintInput(bool virtualSprintState)
    {
        inputs.OnRunning(virtualSprintState);
    }
}