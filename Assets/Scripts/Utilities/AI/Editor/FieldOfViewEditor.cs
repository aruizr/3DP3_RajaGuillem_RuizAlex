using UnityEditor;
using UnityEngine;

namespace Utilities.AI.Editor
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var fov = (FieldOfView) target;
            var transform = fov.transform;
            var position = transform.position;
            var fovPositiveEdge = GetAngleDirection(fov.Angle / 2);
            var fovNegativeEdge = GetAngleDirection(-fov.Angle / 2);

            Handles.color = Color.white;
            Handles.DrawWireArc(position, transform.up, fovNegativeEdge.normalized, fov.Angle, fov.Radius);
            Handles.DrawLine(position, position + fovPositiveEdge * fov.Radius);
            Handles.DrawLine(position, position + fovNegativeEdge * fov.Radius);

            Handles.color = Color.red;
            fov.VisibleTargets.ForEach(t => Handles.DrawLine(position, t.position));
        }

        private Vector3 GetAngleDirection(float degreesAngle)
        {
            var fov = (FieldOfView) target;
            var eulerAngles = fov.transform.eulerAngles;
            var radAngle = (degreesAngle + eulerAngles.y) * Mathf.Deg2Rad;
            var x = Mathf.Sin(radAngle);
            var z = Mathf.Cos(radAngle);
            return Quaternion.Euler(eulerAngles) * new Vector3(x, 0, z);
        }
    }
}