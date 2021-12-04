using UnityEngine;
using Utilities;
using Utilities.Health;
using Utilities.Messaging;
using Utilities.Physics;

[RequireComponent(typeof(RayCaster))]
public class DamageDealer : ExtendedMonoBehaviour, IDamageDealer
{
    [SerializeField] private float damage;

    private RayCaster _rayCaster;

    private void Awake()
    {
        _rayCaster = GetComponent<RayCaster>();
        _rayCaster.OnHitEnter += hit => Messenger.SendAsync<IDamageTaker>(DealDamage, hit.collider.gameObject);
    }

    public void DealDamage(IDamageTaker damageTaker)
    {
        damageTaker.TakeDamage(damage);
    }
}