using MyBox;
using UnityEngine;

namespace Utilities.Physics
{
    public class RayCaster : MonoBehaviour
    {
        [SerializeField] private LayerMask detectLayers;
        [SerializeField] private bool hasDistance;

        [SerializeField] [ConditionalField("hasDistance")]
        private float distance;

        [SerializeField] private Color rayColor = Color.white;
        [SerializeField] [ReadOnly] private bool isColliding;

        public bool IsColliding => isColliding;
        public RaycastHit CurrentHit { get; private set; }

        private void FixedUpdate()
        {
            var t = transform;
            var ray = new Ray(t.position, t.forward);
            isColliding = UnityEngine.Physics.Raycast(ray, out var hit, hasDistance ? distance : Mathf.Infinity, detectLayers);
            CurrentHit = hit;
        }

        private void OnDrawGizmosSelected()
        {
            var t = transform;
            var position = t.position;
            Gizmos.color = rayColor;
            Gizmos.DrawLine(position, position + t.forward * (hasDistance ? distance : 1000));
        }
    }
}