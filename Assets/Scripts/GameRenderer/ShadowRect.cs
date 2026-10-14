using UnityEngine;

namespace GameRenderer
{
    public struct ShadowRect
    {
        public Vector2 Position;
        public Vector2 Size;
        public float ShadowHeight;

        public ShadowRect(Vector2 position, Vector2 size, float shadowHeight) {
            Position = position;
            Size = size;
            ShadowHeight = shadowHeight;
        }
    }
}