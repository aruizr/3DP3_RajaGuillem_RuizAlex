using UnityEngine;
using Utilities;
using Utilities.Health;
using Utilities.Messaging;
using Utilities.Physics;

[RequireComponent(typeof(RayCaster))]
public class PlayerDamageDealer : ExtendedMonoBehaviour, IDamageDealer
{
    [SerializeField] private float damage;
    [SerializeField] private float interval;
    
    private CoroutineBuilder _coroutine;
    private RayCaster _rayCaster;

    private void Awake()
    {
        _rayCaster = GetComponent<RayCaster>();
        _coroutine = Coroutine(destroyOnFinish: false).
            WaitUntil(() => _rayCaster.IsColliding).
            Invoke(() => Messenger.Send<IDamageTaker>(DealDamage, _rayCaster.CurrentHit.collider.gameObject)).
            WaitForSeconds(interval).
            While(() => true);
    }

    private void OnEnable()
    {
        _coroutine.Run();
    }

    public void DealDamage(IDamageTaker damageTaker)
    {
        damageTaker.TakeDamage(damage);
    }
}