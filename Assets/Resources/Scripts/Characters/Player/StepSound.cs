using UnityEngine;
using UnityEngine.InputSystem;

namespace Tcp4
{
    public class StepSound : MonoBehaviour
    {
        public float stepInterval = 0.4f;
        public float deadzone = 0.2f;
        private float stepTimer;
        private InputAction movementAction;
        private bool isMoving;

        private void Awake()
        {
            var playerInput = GetComponent<PlayerInput>();
            movementAction = playerInput.actions["Movement"];
        }

        private void Update()
        {
            Vector2 input = movementAction.ReadValue<Vector2>();
          
            isMoving = input.magnitude > deadzone;

            if (isMoving)
            {
                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0)
                {
                    SoundManager.PlaySound(SoundType.passos, 0.2f);
                    stepTimer = stepInterval;
                }
            }
            else
            {
                stepTimer = 0.1f;
            }
        }
    }
}