using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Attributes;

namespace Utilities.Health
{
    public abstract class HealthSystem : ExtendedMonoBehaviour, IDamageTaker
    {
        [SerializeField] private float health;

        [SerializeField] [ReadOnly] private float currentHealth;

        public float MaxHealth => health;

        public float CurrentHealth
        {
            get => currentHealth;
            protected set
            {
                currentHealth = value < 0 ? 0 : value;
                OnUpdateHealth();
            }
        }

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            OnInit();
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth > 0) OnTakeDamage();
            else OnDie();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnTakeDamage()
        {
        }

        protected virtual void OnDie()
        {
        }

        protected virtual void OnUpdateHealth()
        {
        }
    }
}