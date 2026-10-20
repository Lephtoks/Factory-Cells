using DG.Tweening;
using Economics;
using UnityEngine;

namespace Cells.Object
{
    public struct DroppedItem {
        public Vector2Int Position;
        public Vector2Int TargetPosition;
        public DroppedItem(Vector2Int position, Vector2Int targetPosition) {
            Position = position;
            TargetPosition = targetPosition;
        }
    } 
}