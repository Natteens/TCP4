using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CrimsonReaper
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private Vector2 RawDirection;
        private Vector2 lastRawDirection;
        private int XDirectionNorm;
        private int YDirectionNorm;
        private bool dashInput;
        private bool jumpInput;
        private bool interactInput;
        private bool abilityInput;

        private float inputHoldTime = 0.2f;
        private float dashInputTimer;
        private float jumpInputTimer;

        private bool isJumpButtonHeld;
        private float jumpButtonHoldTime;
        private const float maxJumpHoldTime = 0.5f;

        private void Update()
        {
            HandleDashInput();
            HandleJumpInput();
        }

        #region Movement
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RawDirection = context.ReadValue<Vector2>();
                XDirectionNorm = (int)(RawDirection * Vector2.right).normalized.x;
                YDirectionNorm = (int)(RawDirection * Vector2.up).normalized.y;
                lastRawDirection = RawDirection;
            }
            else if (context.canceled)
            {
                RawDirection = Vector2.zero;
                XDirectionNorm = 0;
                YDirectionNorm = 0;
            }
        }

        public Vector2 GetRawMovementDirection()
        {
            return RawDirection;
        }

        public int GetNormalizedDirectionX()
        {
            return XDirectionNorm;
        }
        public int GetNormalizedDirectionY()
        {
            return YDirectionNorm;
        }

        public Vector2 GetLastMovementDirection()
        {
            return lastRawDirection;
        }
        #endregion

        #region Dash
        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                dashInput = true;
                dashInputTimer = 0f; 
            }
        }

        private void HandleDashInput()
        {
            if (dashInput)
            {
                dashInputTimer += Time.deltaTime;
                if (dashInputTimer > inputHoldTime)
                {
                    dashInput = false;
                    dashInputTimer = 0f;
                }
            }
        }

        public bool GetDashInput()
        {
            return dashInput;
        }
        #endregion

        #region Jump
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                jumpInput = true;
                jumpInputTimer = 0f;
                isJumpButtonHeld = true;
                jumpButtonHoldTime = 0f;
            }
            else if (context.canceled)
            {
                isJumpButtonHeld = false;
                jumpButtonHoldTime = 0f;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInputTimer += Time.deltaTime;
                if (jumpInputTimer > inputHoldTime)
                {
                    jumpInput = false;
                    jumpInputTimer = 0f;
                }
            }
            if (isJumpButtonHeld)
            {
                jumpButtonHoldTime += Time.deltaTime;
                jumpButtonHoldTime = Mathf.Min(jumpButtonHoldTime, maxJumpHoldTime);
            }
        }

        public bool GetJumpInput()
        {
            return jumpInput;
        }

        public float GetJumpHoldTime()
        {
            return jumpButtonHoldTime;
        }

        public bool IsJumpButtonHeld()
        {
            return isJumpButtonHeld;
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

        #region Ability

        public void OnUseAbility(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                abilityInput = true;
            }
            else if (context.canceled)
            {
                abilityInput = false;
            }
        }

        public bool GetAbilityInput()
        {
            return abilityInput;
        }
        #endregion
    }
}
