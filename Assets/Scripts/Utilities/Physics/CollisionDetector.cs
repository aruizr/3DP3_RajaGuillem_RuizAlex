using UnityEngine;

namespace Utilities.Physics
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask detectLayers;
        [SerializeField] [ReadOnly] private bool isColliding;

        private Collider _collider;

        public bool IsColliding => isColliding;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        private void FixedUpdate()
        {
            var bounds = _collider.bounds;
            var results = new Collider[1];
            var size = UnityEngine.Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, results,
                transform.rotation, detectLayers);

            isColliding = size != 0;
        }
    }
}