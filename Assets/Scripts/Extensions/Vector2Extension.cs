using UnityEngine;

namespace Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 Rotate(this Vector2 v, float angle) {
            float rad = angle * Mathf.Deg2Rad;
            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);

            return new Vector2(
                v.x * cos - v.y * sin,
                v.x * sin + v.y * cos
            );
        }
    }
}