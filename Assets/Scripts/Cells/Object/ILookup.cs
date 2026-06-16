using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Cells.Object
{
    public interface ILookup : IPositioned, ICellPlaceable
    {
        IEnumerable<Direction> OutDirections();
        bool TryGetNeighbor(out Block neighbor) {
            foreach (var direction in OutDirections()) {
                if (TryGetNeighbor(direction, out neighbor)) return true;
            }
            neighbor = null;
            return false;
        }
        bool TryGetNeighbor(Vector2Int direction, out Block neighbor) {
            return Parent.TryGetObject(Position + direction, out neighbor);
        }

        bool TryGetNeighbor(Direction direction, out Block neighbor) {
            return TryGetNeighbor(direction.ToVector2Int(), out neighbor);
        }
    }
}