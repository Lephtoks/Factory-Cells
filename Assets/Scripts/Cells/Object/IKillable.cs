using Data;
using UnityEngine;

namespace Cells.Object
{
    public interface IKillable : IHealth
    {
        bool Dead { get; set; }

        public void Kill() {
            Dead = true;
        }

        void IHealth.Damage(float damage) {
            Health -= damage;
            if (Health <= 0) Kill();
            GameEvents.InvokeDamageDealt(this, damage);
        }
    }
}