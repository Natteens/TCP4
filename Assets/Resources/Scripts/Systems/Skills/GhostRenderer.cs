using UnityEngine;

namespace CrimsonReaper
{
    public class GhostRenderer : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float fadeDuration = 1.5f;
        [SerializeField] private Color fadeColor = new Color(0.1f, 0.1f, 0.1f, 1f);
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private SpriteRenderer ghostSpriteRenderer;
        [SerializeField] private StatusEffect effect;
        [SerializeField] private float luck;

        private void Start()
        {
            //player = Player.Instance;
            luck = player.statusComponent.GetStatus(StatusType.Luck);
            ghostSpriteRenderer.sprite = player.spriteRenderer.sprite;
            ghostSpriteRenderer.flipX = player.spriteRenderer.flipX;
            ghostSpriteRenderer.color = fadeColor;
            FadeOut();
        }

        private void FadeOut()
        {
            //  ghostSpriteRenderer.DOColor(new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f), fadeDuration).SetEase(Ease.Linear);
        }

        public void OnDestroy()
        {
            //   ghostSpriteRenderer.DOKill(false);
        }
        public void Init(StatusEffect effect)
        {
            this.effect = effect;
           // player = Player.Instance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (effect != null)
            {
                if ((targetLayer.value & (1 << other.gameObject.layer)) == 0)
                    return;

                StatusComponent defenderStatusComponent = other.gameObject.GetComponent<StatusComponent>();

                if (StatusCalculator.ShouldApplyEffect(luck))
                {
                    defenderStatusComponent.ApplyEffect(effect.CreateEffectData());
                }
            }
        }
    }

}