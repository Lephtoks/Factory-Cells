using UnityEngine;

namespace Cells.Object.Bulding.Mono
{
    public class DefaultRepr<T> : CellNodeRepr<T> where T : CellObject
    {
        public override void Init(T original) {
            MakeReal();
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -1) + new Vector3(0.5f, 0.5f, 0);
        }
    }
}