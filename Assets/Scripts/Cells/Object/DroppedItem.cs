using DG.Tweening;
using Economics;
using UnityEngine;

namespace Cells.Object
{
    public struct DroppedItem {
        public Vector2Int Position;
        public Vector2Int TargetPosition;
        public int SpriteIndex;
        public DroppedItem(Vector2Int position, Vector2Int targetPosition, int spriteIndex = 0) {
            Position = position;
            TargetPosition = targetPosition;
            SpriteIndex = spriteIndex;
        }
    } 
}