using UnityEngine;
using UnityEngine.InputSystem;

namespace Tcp4.Resources.Scripts.Characters.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private Vector3 _rawDirection;
        private bool _interactInput;
        private bool _runningInput;

        #region Movement

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector3 moveInput = context.ReadValue<Vector2>();
                _rawDirection = new Vector3(moveInput.x, 0f,moveInput.y);
            }
            else if (context.canceled)
            {
                _rawDirection = Vector3.zero;
            }
        }

        public Vector3 GetRawMovementDirection()
        {
            return _rawDirection;
        }

        public Vector3 OnGetRawMovement(Vector3 rawDirection)
        {
           return _rawDirection = rawDirection;
        }

        #endregion

        #region Interaction

        public void OnInteract(InputAction.CallbackContext context)
        {
            _interactInput = context.performed;
        }

        public bool GetInteractInput()
        {
            return _interactInput;
        }

        public bool OnInteract(bool interactInput)
        {
            return _interactInput = interactInput;
        }

        #endregion

        #region Running

        public void OnRunning(InputAction.CallbackContext context)
        {
            _runningInput = context.performed;
        }

        public bool GetRunningInput()
        {
            return _runningInput;
        }
        
        public bool OnRunning(bool runInput)
        {
            return _runningInput = runInput;
        }
        
        #endregion
    }
}