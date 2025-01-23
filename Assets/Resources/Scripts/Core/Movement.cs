using Tcp4.Resources.Scripts.Core;
using UnityEngine;

namespace Tcp4
{
    public class Movement
    {
        private Rigidbody Rb;
        private Transform entityTransform;
        private float rotationSpeed = 10f;

        public Movement(DynamicEntity entity)
        {
            this.Rb = entity.Rb;
            this.entityTransform = entity.transform;
        }

        public void Move(Vector3 direction, float moveSpeed)
        {
            if (direction.magnitude > 0)
            {
                Vector3 movement = direction.normalized * moveSpeed;
                Rb.linearVelocity = new Vector3(movement.x, Rb.linearVelocity.y, movement.z);
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                entityTransform.rotation = Quaternion.Slerp(
                    entityTransform.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );
            }
            else
            {
                Rb.linearVelocity = new Vector3(0f, Rb.linearVelocity.y, 0f);
            }
        }

        public Vector3 GetFacingDirection()
        {
            return entityTransform.forward;
        }
    }
}