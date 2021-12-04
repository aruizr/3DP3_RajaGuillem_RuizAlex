using UnityEngine;
using Utilities;
using Utilities.Health;
using Utilities.Messaging;
using Utilities.Physics;

public class Puncher : ExtendedMonoBehaviour, IDamageDealer
{
    [SerializeField] private float damage;
    [SerializeField] private float force;
    [SerializeField] private RayCaster rayCaster;

    public void DealDamage(IDamageTaker damageTaker)
    {
        damageTaker.TakeDamage(damage);
    }

    public void Punch()
    {
        if (!rayCaster.IsColliding) return;
        var target= rayCaster.CurrentHit.collider.gameObject;
        Messenger.SendAsync<IDamageTaker>(DealDamage, target.gameObject);
        Messenger.SendAsync<Rigidbody>(ApplyForce, target.gameObject);
    }

    private void ApplyForce(Rigidbody rb)
    {
        var direction = DirectionTo(rb.transform.position);
        rb.AddForce(direction * force);
    }
}