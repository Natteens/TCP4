using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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

        public void Moving()    {StartCoroutine(nameof(MovingCoroutine));}
        IEnumerator MovingCoroutine()
        {
            isMoving = true;
            yield return new WaitForSeconds(0.2f);
            isMoving = false;
        }

        private void Update()
        {
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