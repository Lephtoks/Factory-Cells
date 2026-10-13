using Cells;
using Cells.Object;
using Collider;
using Extensions;
using UnityEngine;

namespace Entities
{
    public class Bullet : ICellPlaceable, IFloatPositioned, ICollider, IRotated
    {
        public Bullet(Cell parent, Vector2 position, float rotation, BulletType bulletType) {
            Position = position;
            Parent = parent;
            BulletType = bulletType;
            Rotation = rotation;
            Hitbox = new RectHitbox(() => Position, this, Vector2.zero,Vector2.one * 0.5f);
            LifeTime = 5f;
        }

        public bool Dead { get; private set; }
        public Cell Parent { get; }
        public readonly BulletType BulletType;
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Hitbox Hitbox { get; }
        private float _livingTime;
        public float LifeTime { get; set; }
        public void Collision(ICollider other) {
            Kill();
        }

        public void Kill() {
            Dead = true;
        }

        public void Update() {
            _livingTime += Time.deltaTime;
            if (_livingTime > LifeTime) {
                Kill();
            }
            Position += (Vector2.up * (Time.deltaTime * 2f)).Rotate(Rotation);
        }

        public bool CanCollide(ICollider other) {
            return other is not Bullet;
        }
    }
}