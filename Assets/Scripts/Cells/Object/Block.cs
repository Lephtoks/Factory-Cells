using UnityEngine;

namespace Cells.Object
{
    public abstract class Block : IPositioned, ICellPlaceable
    {
        public Cell Parent { get; }
        public Vector2Int Position { get; }

        public Block(Cell parent, Vector2Int pos) {
            Position = pos;
            Parent = parent;
        }
        public virtual void UpdateMove() {}
        
        public virtual void WhenBeingAddedToCell() {
        }
    }
}