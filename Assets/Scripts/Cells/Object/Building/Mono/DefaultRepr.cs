using UnityEngine;

namespace Cells.Object.Building.Mono
{
    public class DefaultRepr : BlockRepr
    {
        public override void Init(Block original) {
            MakeReal();
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -1) + new Vector3(0.5f, 0.5f, 0);
        }
    }
}