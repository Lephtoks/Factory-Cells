using Data;
using DefaultNamespace;
using UnityEngine;

namespace Cells.Object
{
    public abstract class BlockRepr : TransparencyGroup
    {
        public void MakePhantom() {
            gameObject.SetActive(true);
            SetAlpha(0.5f);
        }

        public void MakeReal() {
            gameObject.SetActive(true);
            SetAlpha(1f);
        }

        public void MakeInvisible() {
            gameObject.SetActive(false);
        }

        public void SetPos(Vector2 pos) {
            transform.position = pos;
        }

        public void SetPos(Vector3Int pos, Transform cell) {
            transform.SetParent(cell, false);
            transform.localPosition = new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z);
        }

        public virtual void UseSettings(RepresentationSettings representationSettings) {
        }
        public abstract void Init(Block repr);
    }
    public abstract class BlockRepr<T> : BlockRepr where T : Block
    {
        public sealed override void Init(Block repr) {
            Init((T) repr);
        }
        public abstract void Init(T original);

    }
}