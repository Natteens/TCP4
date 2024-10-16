using System;
using UnityEngine;

namespace Tcp4
{
    public class Movement
    {
        private Rigidbody rb;
        private SpriteRenderer spriteRenderer;
        private bool isFacingLeft;

        public Movement(DynamicEntity entity)
        {
            this.rb = entity.rb;
            this.spriteRenderer = entity.spriteRenderer;
            //this.knockback = entity.knockback;
        }

        public void Move(int direction, float moveSpeed)
        {
            //if (knockback.GettingKnockedBack) return;
            Vector2 movement = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = movement;
            Flip(direction);
        }

        public void Flip(int moveInput)
        {
            if (moveInput < 0 && !isFacingLeft)
            {
                FlipCharacter();
            }
            else if (moveInput > 0 && isFacingLeft)
            {
                FlipCharacter();
            }
        }

        private void FlipCharacter()
        {
            isFacingLeft = !isFacingLeft;
            spriteRenderer.flipX = isFacingLeft;
        }
    }
}
