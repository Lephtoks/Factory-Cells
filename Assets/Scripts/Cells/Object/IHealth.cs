using UnityEngine;

namespace Cells.Object
{
    public interface IHealth
    {
        float MaxHealth { get; }
        float Health { set; get; }

        public void Damage(float damage) {
            Health -= damage;
        }

        public void Heal(float heal) {
            Health = Mathf.Min(heal + Health, MaxHealth);
        }
    }
}