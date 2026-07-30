using System;
using Entities;
using UnityEngine;

namespace Collider
{
    public class RectHitbox : Hitbox
    {
        public Vector2 Offset;
        public Vector2 Size;
        public RectHitbox(Func<Vector2> transformProvider, ICollider owner, Vector2 offset, Vector2 size) : base(transformProvider, owner) {
            Size = size;
            Offset = offset;
        }

        public bool Intersects(Hitbox other) {
            return Hitbox.Intersects(this, other);
        } 
    }
}