using UnityEngine;

namespace Utilities.Unity
{
    public static class UnityHelper
    {
        public static bool IsAnyObjectAtPosition(Vector3 position, float radius)
        {
            var collider = Physics2D.OverlapCircle(position, radius);
            return collider is not null;
        }

        public static bool Approximately(float a, float b, float precision) => Mathf.Abs(b - a) <= precision;

        public static bool Approximately(Vector3 original, Vector3 other, float precision) => 
            Approximately(original.x, other.x, precision) && 
            Approximately(original.y, other.y, precision);

        public static bool VectorAngleLessThanAngle(Vector3 targetDirection, Vector3 currentDirection, float angle)
        {
            return Vector2.SignedAngle(targetDirection, currentDirection) <= angle;
        }
        
        public static bool DirectionInsideAngle(Vector3 targetDirection, Vector3 currentDirection, float angle)
        {
            return Mathf.Abs(Vector2.SignedAngle(targetDirection, currentDirection)) <= angle / 2;
        }

        public static bool IsObjectVisible(this Camera camera, Bounds bounds)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), bounds);
        }

        public static Vector2 GetAPointOnRadius(Vector2 referencePoint, float radius)
        {
            GameObject tempTransform = new();

            var tempRotation = Random.Range(0, 359);

            tempTransform.transform.position = new Vector3(referencePoint.x, referencePoint.y, 0);
            tempTransform.transform.RotateAround(referencePoint, Vector3.forward, tempRotation);
            tempTransform.transform.Translate(Vector2.up * radius, Space.Self);

            var output = tempTransform.transform.position;
            Object.Destroy(tempTransform);

            return output;
        }
    }
}