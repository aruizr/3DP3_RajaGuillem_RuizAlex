using UnityEngine;
using Utilities;
using Utilities.Health;
using Utilities.Messaging;

public class Puncher : ExtendedMonoBehaviour, IDamageDealer
{
    [SerializeField] private float damage;
    [SerializeField] private float force;
    [SerializeField] private FieldOfView fov;

    public void DealDamage(IDamageTaker damageTaker)
    {
        damageTaker.TakeDamage(damage);
    }

    public void Punch()
    {
        foreach (var target in fov.VisibleTargets)
        {
            Messenger.SendAsync<IDamageTaker>(DealDamage, target.gameObject);
            Messenger.SendAsync<Rigidbody>(ApplyForce, target.gameObject);
        }
    }

    private void ApplyForce(Rigidbody rb)
    {
        var direction = DirectionTo(rb.transform.position);
        rb.AddForce(direction * force);
    }
}