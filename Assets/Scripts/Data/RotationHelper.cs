using UnityEngine;

namespace Data
{
    public static class RotationHelper
    {
        public static Quaternion RotateQ(float angle, float targetAngle, float maxDelta) {
            float delta = Mathf.DeltaAngle(angle,targetAngle);

            if (Mathf.Abs(delta) >= 0.1f) {
                angle += Mathf.Sign(delta) * Mathf.Min(Mathf.Abs(delta), maxDelta);
                return Quaternion.Euler(0, 0, Mathf.Repeat(angle + 180, 360) - 180);
            }
            return Quaternion.Euler(0, 0, angle);
        }
        public static float RotateF(float angle, float targetAngle, float maxDelta) {
            float delta = Mathf.DeltaAngle(angle,targetAngle);

            if (Mathf.Abs(delta) >= 0.1f) {
                angle += Mathf.Sign(delta) * Mathf.Min(Mathf.Abs(delta), maxDelta);
                return Mathf.Repeat(angle + 180, 360) - 180;
            }

            return angle;
        }

        public static float AngleTo(Vector2 from, Vector2 to) {
            var dir = to - from;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        }
        
        public static float Limit(float angle, float target, float radius) {
            float delta = Mathf.DeltaAngle(target, angle);

            delta = Mathf.Clamp(delta, -radius, radius);

            return Mathf.Repeat(target + delta + 180f, 360f) - 180f;
        }
    }
}