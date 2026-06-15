using Data;
using UnityEngine;

namespace Cells.Object
{
    public abstract class DirectedCellNode<T, K> : CellNode<T, K> where T : CellNodeRepr<K> where K : CellObject
    {
        private readonly Direction _direction;
        public DirectedCellNode(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos) {
            _direction = direction;
        }
        
        public Direction GetDirection() => _direction;
    }
}