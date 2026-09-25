using System;
using Data;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;

namespace Cells.Object
{
    public abstract class BlockRepr : TransparencyGroup
    {
        private static readonly Color WrongColor = new Color(1, 0, 0, 0.65f);
        [CanBeNull] [NonSerialized] public Cell Cell;
        public BlockType BlockType;
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
        public void MakeWrong() {
            gameObject.SetActive(true);
            SetColor(WrongColor);
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