using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tcp4
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private Vector3 RawDirection;
        private bool interactInput;

        #region PC
        #region Movement
        public void OnMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 moveInput = context.ReadValue<Vector2>();
                RawDirection = new Vector3(moveInput.x, 0, moveInput.y);
            }
            else if (context.canceled)
            {
                RawDirection = Vector3.zero;
            }
        }
        public Vector3 GetRawMovementDirection()
        {
            return RawDirection;
        }

        #endregion

        #region Interaction

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                interactInput = true;
            }
            else if (context.canceled)
            {
                interactInput = false;
            }
        }
        public bool GetInteractInput()
        {
            return interactInput;
        }

        #endregion 
        #endregion
    }
}
