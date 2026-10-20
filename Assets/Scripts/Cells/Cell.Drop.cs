using System.Collections.Generic;
using Cells.Object;
using Core;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        public void UpdateDropPositions() {
            foreach (var key in _dropRenderer.GetValues()) {
                _dropRenderer.Remove(key.Position);
                _dropRenderer.Add(key.TargetPosition, new DroppedItem(key.TargetPosition, key.TargetPosition));
            }
        }

        public void AddDrop(DroppedItem droppedItem) {
            Debug.Log($"Adding drop {droppedItem.Position}:{droppedItem.TargetPosition}");
            _dropRenderer.Add(droppedItem.Position, droppedItem);
        }
    }
}