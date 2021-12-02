using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class FieldOfView : ExtendedMonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] [Range(0, 360)] private float angle;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private float targetScanRate;

    private CoroutineBuilder _scanCoroutine;

    public float Radius => radius;
    public float Angle => angle;
    public List<Transform> VisibleTargets { get; private set; } = new List<Transform>();

    private void Awake()
    {
        _scanCoroutine = Coroutine(cancelOnDisable: true, destroyOnFinish: false).
            Invoke(ScanVisibleTargets).
            WaitForSeconds(1 / targetScanRate).
            While(() => true);
    }

    private void OnEnable()
    {
        _scanCoroutine.Run();
    }

    private void ScanVisibleTargets()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayers);

        VisibleTargets = (from coll in colliders
            select coll.transform
            into target
            let direction = (target.position - transform.position).normalized
            where IsInRange(direction)
            let distance = Vector3.Distance(transform.position, target.position)
            where !IsObstacle(direction, distance)
            select target).ToList();
    }

    private bool IsObstacle(Vector3 direction, float distance)
    {
        return Physics.Raycast(transform.position, direction, distance, obstacleLayers);
    }

    private bool IsInRange(Vector3 direction)
    {
        return Vector3.Angle(transform.forward, direction) <= angle * 0.5f;
    }
}