using Utilities.Health;
using Utilities.Messaging;

namespace Player
{
    public class PlayerHealthSystem : HealthSystem
    {
        protected override void OnTakeDamage()
        {
            EventManager.TriggerEvent("OnPlayerHit", new Message(this));
            EventManager.TriggerEvent("OnUpdatePlayerHealth", new Message(this) {{"health", CurrentHealth}});
        }

        protected override void OnDie()
        {
            EventManager.TriggerEvent("OnPlayerDie", new Message(this));
        }
    }
}