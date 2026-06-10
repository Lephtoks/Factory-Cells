using Data;
using UnityEngine;

namespace Cells.Object
{
    public abstract class DirectedCellNode<T, K> : CellNode<T, K> where T : CellNodeRepr<K> where K : CellObject
    {
        private readonly Direction _direction;
        public DirectedCellNode(Cell parent, T repr) : base(parent, repr) {
            _direction = DirectionHelper.Vector2Direction(repr.transform.localRotation * Vector3.forward);
        }
        
        public Direction GetDirection() => _direction;
    }
}