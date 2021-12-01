namespace Utilities.Health
{
    public interface IDamageDealer
    {
        void DealDamage(IDamageTaker damageTaker);
    }
}