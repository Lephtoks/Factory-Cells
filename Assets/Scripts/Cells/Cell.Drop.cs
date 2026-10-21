using System.Collections.Generic;
using System.Linq;
using Cells.Object;
using Core;
using NUnit.Framework;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        public void UpdateDropPositions() {
            var ignore = new List<Vector2Int>();
            var droppedItems = _dropRenderer.GetValues();
            foreach (var key in droppedItems) {
                ignore.Add(key.TargetPosition);
                _dropRenderer.Add(key.TargetPosition, new DroppedItem(key.TargetPosition, key.TargetPosition));
            }

            foreach (var val in droppedItems.Select((val) => val.Position).Except(ignore)) {
                _dropRenderer.Remove(val);
            }
            
        }

        public void AddDrop(DroppedItem droppedItem) {
            _dropRenderer.Add(droppedItem.Position, droppedItem);
        }
    }
}