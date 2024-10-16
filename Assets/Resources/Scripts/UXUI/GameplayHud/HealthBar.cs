using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrimsonReaper
{
    public class HealthBar : MonoBehaviour
    {
        public TextMeshProUGUI healthText;
        public Image healthBar;
        public Image damageBar;
        public HealthComponent healthComponent;

        float health, maxHealth;
        float previousHealth;

        [SerializeField] private Color damageColor = Color.yellow;
        [SerializeField] private Color healColor = Color.green;
        [SerializeField] private float damageLerpSpeed = 0.5f;

        private void Start()
        {
            InitializeHealthBar();
            healthComponent.OnTakeDamage += Damage;
            healthComponent.OnHeal += Heal;
            previousHealth = healthComponent.CurrentHealth;
        }

        private void OnDestroy()
        {
            healthComponent.OnTakeDamage -= Damage;
            healthComponent.OnHeal -= Heal;
        }

        private void Update()
        {
            UpdateHealthBarUI();
            HealthToText();
        }

        private void InitializeHealthBar()
        {
            maxHealth = healthComponent.MaxHealth;
            health = healthComponent.CurrentHealth;
            previousHealth = healthComponent.CurrentHealth;
            UpdateHealthBarUI();
        }

        private void HealthToText()
        {
            healthText.text = $"{health} / {maxHealth}";
        }

        private void UpdateHealthBarUI()
        {
            healthBar.fillAmount = health / maxHealth;
            LerpDamageBar();
        }

        private void LerpDamageBar()
        {
            damageBar.fillAmount = Mathf.Lerp(damageBar.fillAmount, health / maxHealth, damageLerpSpeed * Time.deltaTime);
        }

        public void Damage(float damagePoints)
        {
            if (health > 0)
            {
                health -= damagePoints;
                if (health <= 0) health = 0;
            }
            damageBar.color = damageColor;
            UpdateHealthBarUI();
        }

        public void Heal(float healingPoints)
        {
            health -= healingPoints;
            if (health < maxHealth)
            {
                health += healingPoints;
                if (health >= maxHealth) health = maxHealth;
            }
            damageBar.color = healColor;
            UpdateHealthBarUI();
        }
    }

}