using System;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object
{
    public abstract class CellObject : IPositioned, ICellPlaceable
    {
        public Cell Parent { get; }
        public Vector2Int Position { get; }

        public CellObject(Cell parent, Vector2Int pos) {
            Position = pos;
            Parent = parent;
        }
        public virtual void UpdateMove() {}
        
        public virtual void WhenBeingAddedToCell() {
        }
    }
}