using UnityEngine;

namespace Cells.Object
{
    public abstract class Block : IPositioned, ICellPlaceable
    {
        public Cell Parent { get; set; }
        public Vector2Int Position { get; private set; }
        public abstract BlockType BlockType { get; }
        public bool Destroyed = false;
        public bool IsGhost = true;

        public Block() {
        }
        public virtual void UpdateMove() {}

        public virtual void Init(Vector2Int pos) {
            Position = pos;
            IsGhost = false;
        }
        public virtual void WhenBeingAddedToCellLate() { }
        public virtual void OnDestroy() {}

        public Block Duplicate() {
            return (Block)MemberwiseClone();
        }
    }
}