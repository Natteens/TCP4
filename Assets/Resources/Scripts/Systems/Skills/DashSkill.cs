using System.Collections;
using UnityEngine;

namespace CrimsonReaper
{
    [CreateAssetMenu(fileName = "New Dash Skill", menuName = "Skills/Dash")]
    public class DashSkill : BaseSkill
    {
        public float dashSpeed = 10f;
        public float dashDuration = 0.5f;
        public LayerMask invulnerableLayers;
        public GameObject effectPrefab;
        public bool leavesTrail;
        public StatusEffect trailEffect;
        public int numberOfTrails = 3;

        public override void ExecuteSkill(DynamicEntity entity)
        {
            Rigidbody rb = entity.GetComponent<Rigidbody>();
            Collider collider = entity.GetComponent<Collider>();
            PlayerInputHandler input = entity.GetComponent<PlayerInputHandler>();
            Vector2 moveInput = input.GetRawMovementDirection();
            entity.anim.SetTrigger("IsDashing");
            Vector2 dashDirection = moveInput.sqrMagnitude == 0 ? input.GetLastMovementDirection() : moveInput.normalized;

            entity.StartCoroutine(Dash(rb, collider, dashDirection));
        }

        private IEnumerator Dash(Rigidbody rb, Collider collider, Vector2 dashDirection)
        {
            LayerMask originalExcludeLayers = collider.excludeLayers;
            collider.excludeLayers = invulnerableLayers;

            rb.linearVelocity = dashDirection * dashSpeed;
            isUsing = true;

            for (int i = 0; i < numberOfTrails; i++)
            {
                if (leavesTrail && effectPrefab != null)
                {
                    GameObject trail = Instantiate(effectPrefab, rb.position, Quaternion.identity);
                    trail.GetComponent<GhostRenderer>().Init(trailEffect);
                    Destroy(trail, dashDuration);
                }
                yield return new WaitForSeconds(dashDuration / numberOfTrails);
            }

            rb.linearVelocity = Vector2.zero;
            isUsing = false;
            collider.excludeLayers = originalExcludeLayers;
        }
    }

}