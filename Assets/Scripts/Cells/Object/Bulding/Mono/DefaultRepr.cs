using Data;
using UnityEngine;

namespace Cells.Object.Bulding.Mono
{
    public class DefaultRepr<T> : CellNodeRepr<T> where T : CellObject
    {
        public override void Init(T original) {
            transform.parent = original.Parent.transform;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -1) + new Vector3(0.5f, 0.5f, 0);
        }
    }
}