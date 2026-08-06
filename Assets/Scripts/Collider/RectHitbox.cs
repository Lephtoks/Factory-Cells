using System;
using Cells.Object;
using Entities;
using GameDebug;
using UnityEngine;

namespace Collider
{
    public class RectHitbox : Hitbox
    {
        public Vector2 Offset;
        public Vector2 Size;
        public RectHitbox(Func<Vector2> transformProvider, ICollider owner, Vector2 offset, Vector2 size) : base(owner, transformProvider) {
            Size = size;
            Offset = offset;
        }

        public bool Intersects(Hitbox other) {
            return Hitbox.Intersects(this, other);
        }
        public override void Draw()
        {
            if (Owner is ICellPlaceable cellPlaceable) {
                DebugDrawer.Rect(cellPlaceable.Parent.tilemap.LocalToWorld(TransformProvider() + Offset),
                    Size * cellPlaceable.Parent.tilemap.transform.lossyScale,
                    Color.green);
            }
        }
    }
}