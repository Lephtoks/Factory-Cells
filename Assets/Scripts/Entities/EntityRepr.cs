using System;
using Data;
using UnityEngine;

namespace Entities
{
    public abstract class EntityRepr : MonoBehaviour
    {
        public void SetPos(Vector2 pos) {
            transform.localPosition = new  Vector3(pos.x, pos.y, transform.position.z);
        }

        public void SetPos(Vector3 pos, Transform cell) {
            transform.SetParent(cell, false);
            transform.localPosition = pos;
        }
    }
    public abstract class EntityRepr<T> : EntityRepr where T : Entity
    {
        [NonSerialized] public T Parent;

        public virtual void Init(T original) {
            Parent = original;
        }

    }
}