using System;
using UnityEngine;

namespace Tcp4
{
    public class Movement
    {
        private Rigidbody rb;
        private Transform entityTransform;

        public Movement(DynamicEntity entity)
        {
            this.rb = entity.rb;
            this.entityTransform = entity.transform;
        }

        public void Move(Vector3 direction, float moveSpeed)
        {
            Vector3 movement = direction.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
        }

        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - entityTransform.position;
            direction.y = 0f;

            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                entityTransform.rotation = Quaternion.Slerp(entityTransform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}
