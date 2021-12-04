using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.AI;

namespace Utilities.AI
{
    public class AreaDetector : ExtendedMonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] [Range(0, 360)] private float angle;
        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private LayerMask obstacleLayers;
        [SerializeField] private float scanRate;
        [SerializeField] private Color gizmosColor = Color.white;

        private CoroutineBuilder _scanCoroutine;

        public float Radius => radius;
        public float Angle => angle;
        public Color GizmosColor => gizmosColor;
        public List<Transform> DetectedTargets { get; private set; } = new List<Transform>();

        private void Awake()
        {
            _scanCoroutine = Coroutine(cancelOnDisable: true, destroyOnFinish: false).Invoke(ScanVisibleTargets)
                .WaitForSeconds(1 / scanRate).While(() => true);
        }

        private void OnEnable()
        {
            _scanCoroutine.Run();
        }

        private void ScanVisibleTargets()
        {
            var colliders = UnityEngine.Physics.OverlapSphere(transform.position, radius, targetLayers);

            DetectedTargets = (from coll in colliders
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
            return UnityEngine.Physics.Raycast(transform.position, direction, distance, obstacleLayers);
        }

        private bool IsInRange(Vector3 direction)
        {
            return Vector3.Angle(transform.forward, direction) <= angle * 0.5f;
        }
    }
}

namespace UnityEditor
{
    [CustomEditor(typeof(AreaDetector))]
    public class AreaDetectorEditor : Editor
    {
        private void OnSceneGUI()
        {
            var detector = (AreaDetector) target;
            var transform = detector.transform;
            var position = transform.position;
            var fovPositiveEdge = GetAngleDirection(detector.Angle / 2);
            var fovNegativeEdge = GetAngleDirection(-detector.Angle / 2);

            Handles.color = detector.GizmosColor;
            Handles.DrawWireArc(position, transform.up, fovNegativeEdge.normalized, detector.Angle, detector.Radius);
            Handles.DrawLine(position, position + fovPositiveEdge * detector.Radius);
            Handles.DrawLine(position, position + fovNegativeEdge * detector.Radius);

            Handles.color = Color.red;
            detector.DetectedTargets.ForEach(t => Handles.DrawLine(position, t.position));
        }

        private Vector3 GetAngleDirection(float degreesAngle)
        {
            var fov = (AreaDetector) target;
            var eulerAngles = fov.transform.eulerAngles;
            var radAngle = (degreesAngle + eulerAngles.y) * Mathf.Deg2Rad;
            var x = Mathf.Sin(radAngle);
            var z = Mathf.Cos(radAngle);
            return Quaternion.Euler(eulerAngles) * new Vector3(x, 0, z);
        }
    }
}