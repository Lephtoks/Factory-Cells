using UnityEngine;

namespace Cells.Object.Building.Mono
{
    public class DefaultRepr<T> : BlockRepr<T> where T : Block
    {
        public override void Init(T original) {
            MakeReal();
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -1) + new Vector3(0.5f, 0.5f, 0);
        }
    }
}