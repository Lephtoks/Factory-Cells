using System.Collections.Generic;
using Cells;
using Cells.Object;
using Collider;
using Data;
using Entities.Navigation;
using UnityEngine;

namespace Entities
{
    public class Entity : ICellPlaceable, IFloatPositioned, ICollider, IHealth
    {
        public Cell Parent { get; }
        public virtual Vector2 Position { get; set; }
        public Hitbox Hitbox { get; }
        public bool Dead = false;
        public float MaxHealth { get; set; }
        public float Health { get; set; }

        public virtual float Angle { get; set; }
        public float PathAngle;
        public Vector2 Target;
        private float _rotationSpeed = 30f;
        private List<NavNode> _path;
        private NavNode _currentNode;
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
            time = 0;
        }
        
        public void Update() {
            Target = Parent.tilemap.WorldToLocal(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            time += Time.deltaTime;
            if (_path == null || time >= 0.05f) {
                Pathfind();
                time = 0;
            }
            
            Angle = RotationHelper.RotateF(Angle, PathAngle, _rotationSpeed * Time.deltaTime);

            if (Vector2.Distance(_currentNode.Position, Position) < 0.1f) {
                int indexOf = _path.IndexOf(_currentNode);
                if (_path.Count > indexOf+1) {
                    _currentNode = _path[indexOf + 1];
                }
            }
            PathAngle = RotationHelper.AngleTo(Position, _currentNode.Position);
            Position += (_currentNode.Position - Position).normalized * (0.5f * Time.deltaTime);
            
            
        }

        private void Pathfind() {
            Vector3 worldToLocal = Target;
            Debug.Log(worldToLocal);
            _path = Parent.NavTree.BuildPath(Position, worldToLocal);
            if (_path.Count > 0) {
                _currentNode = _path[0];
            }
        }
    }
}