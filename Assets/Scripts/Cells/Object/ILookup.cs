using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Cells.Object
{
    public interface ILookup : IPositioned, ICellPlaceable
    {
        IEnumerable<Direction> OutDirections();
        IEnumerable<Direction> ChooseDirection();
        bool TryGetNeighbor(out Block neighbor) {
            foreach (var direction in ChooseDirection()) {
                if (TryGetNeighbor(direction, out neighbor)) return true;
            }
            neighbor = null;
            return false;
        }
        bool TryGetReceiver(out IInventory inventory) {
            inventory = null;
            foreach (var direction in ChooseDirection()) {
                if (TryGetNeighbor(direction, out Block neighbor) && neighbor is IInventory { CanReceive: true } node) {
                    inventory = node;
                    return true;
                }
            }
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