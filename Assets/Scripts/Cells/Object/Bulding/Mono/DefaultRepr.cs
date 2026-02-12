using Data;
using UnityEngine;

namespace Cells.Object.Bulding.Mono
{
    public class DefaultRepr<T> : CellNodeRepr<T> where T : CellObject
    {
        public override void Init(T original) {
            transform.position = new Vector3(original.Position.x, original.Position.y, -1);
        }
    }
}