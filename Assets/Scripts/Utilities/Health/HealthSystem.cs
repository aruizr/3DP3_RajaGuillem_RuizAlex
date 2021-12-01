using UnityEngine;

namespace Utilities.Health
{
    public abstract class HealthSystem : ExtendedMonoBehaviour, IDamageTaker
    {
        [SerializeField] private float health;

        private float _currentHealth;

        public float MaxHealth => health;

        public float CurrentHealth
        {
            get => _currentHealth;
            protected set
            {
                _currentHealth = value < 0 ? 0 : value;
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