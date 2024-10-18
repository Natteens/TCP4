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
            if (direction.magnitude > 0.1f)
            {
                Vector3 movement = direction.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                entityTransform.rotation = Quaternion.Slerp(
                    entityTransform.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
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