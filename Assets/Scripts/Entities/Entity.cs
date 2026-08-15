using System.Collections.Generic;
using Cells;
using Cells.Object;
using Collider;
using Entities.Navigation;
using UnityEngine;

namespace Entities
{
    public class Entity : ICellPlaceable, IFloatPositioned, ICollider, IHealth
    {
        public Cell Parent { get; }
        public Vector2 Position { get; set; }
        public Hitbox Hitbox { get; }
        public bool Dead = false;
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        
        
        private List<NavNode> _path;
        private int index;
        private float time;
        
        public void Collision(ICollider other) {
            if (other is Bullet bullet) {
                BulletCollision(bullet);
            }
        }

        public void Damage(float amount) {
            Health -= amount;
            if (Health <= 0) {
                Kill();
            }
        }

        public void Kill() { 
            Dead = true;
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
            if (_path == null) {
                _path = Parent.
                    NavTree.
                    BuildPath(Position, Vector2.zero);
                index = 0;
                time = 0;
            }
            time += Time.deltaTime;
            if (time > 1f) {
                index++;
                if (index >= _path.Count) {
                    index = 0;
                }
                time = 0;
                Position = _path[index].Position;
            }
            
        }
    }
}