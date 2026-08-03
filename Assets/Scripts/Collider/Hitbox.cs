using System;
using Entities;
using UnityEngine;

namespace Collider
{
    public class Hitbox
    {
        public ICollider Owner { get; }
        private Func<Vector2> _transformProvider;
        public Hitbox(ICollider owner, Func<Vector2> transformProvider) {
            _transformProvider = transformProvider;
            Owner = owner;
        }
        public static bool Intersects(Hitbox a, Hitbox b) {
            switch (a) {
                case RectHitbox aRect when b is RectHitbox bRect:
                    return RectVsRect(aRect, bRect);
                default:
                    throw new System.NotImplementedException("This hitbox intersection is not implemented.");
            }
        }

        public static void Collide(Hitbox a, Hitbox b) {
            if (Intersects(a, b)) {
                a.Owner.Collision(b.Owner);
                b.Owner.Collision(a.Owner);
            }
        }

        private static bool RectVsRect(RectHitbox aRect, RectHitbox bRect) {
            
            Vector2 halfA = aRect.Size * 0.5f;
            Vector2 halfB = bRect.Size * 0.5f;

            var offsetA = aRect.Offset + aRect._transformProvider();
            var offsetB = bRect.Offset + bRect._transformProvider();
            
            return Mathf.Abs(offsetA.x - offsetB.x) <= halfA.x + halfB.x &&
                   Mathf.Abs(offsetA.y - offsetB.y) <= halfA.y + halfB.y;
        }
    }
}