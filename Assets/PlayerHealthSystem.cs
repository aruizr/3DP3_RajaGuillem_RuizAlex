using Utilities.Health;
using Utilities.Messaging;

public class PlayerHealthSystem : HealthSystem
{
    protected override void OnTakeDamage()
    {
        EventManager.TriggerEvent("OnPlayerTakeDamage", new Message(this));
    }

    protected override void OnDie()
    {
        EventManager.TriggerEvent("OnPlayerDie", new Message(this));
    }
}