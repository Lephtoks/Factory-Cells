using Collider;
using UnityEngine;

namespace Entities
{
    public interface ICollider
    {
        public Hitbox Hitbox { get; }

        public void Collision(ICollider other);
    }
}