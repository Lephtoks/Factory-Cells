using Cells;
using Cells.Object;
using Collider;
using UnityEngine;

namespace Entities
{
    public class Entity : ICellPlaceable, IFloatPositioned, ICollider, IHealth
    {
        public Cell Parent { get; }
        public Vector2 Position { get; }
        public Hitbox Hitbox { get; }
        public void Collision(ICollider other) {
            if (other is Bullet bullet) {
                BulletCollision(bullet);
            }
        }

        public void Damage(float amount) {
            Health -= amount;
        }

        private void BulletCollision(Bullet bullet) {
            Damage(10);
        }

        public Entity(Cell parent, Vector2 position) {
            Parent = parent;
            Position = position;
            Hitbox = new RectHitbox(() => Position, this, Vector2.zero, Vector2.one);
        }
        
        public void Update() {
            
        }

        public float MaxHealth { get; set; }
        public float Health { get; set; }
    }
}