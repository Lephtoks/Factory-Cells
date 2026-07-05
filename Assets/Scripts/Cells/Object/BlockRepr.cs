using Data;
using DefaultNamespace;
using UnityEngine;

namespace Cells.Object
{
    public abstract class BlockRepr : TransparencyGroup, IBlockRepr
    {
        public Texture2D textureForUI;
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
    }
    public abstract class BlockRepr<T> : BlockRepr where T : Block
    {
        public abstract void Init(T original);

    }
}