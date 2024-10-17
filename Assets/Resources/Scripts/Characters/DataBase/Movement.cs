using UnityEngine;

namespace Tcp4
{
    public class Movement
    {
        private Rigidbody rb;
        private Transform entityTransform;
        private float rotationSpeed = 10f;

        public Movement(DynamicEntity entity)
        {
            this.rb = entity.rb;
            this.entityTransform = entity.transform;
        }

        public void Move(Vector3 direction, float moveSpeed)
        {
            // Aplicar movimento
            if (direction.magnitude > 0.1f)
            {
                // Movimento simples no plano XZ
                Vector3 movement = direction.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

                // Rotacionar o personagem na direção do movimento
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                entityTransform.rotation = Quaternion.Slerp(
                    entityTransform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
            else
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }

        public Vector3 GetFacingDirection()
        {
            return entityTransform.forward;
        }
    }
}