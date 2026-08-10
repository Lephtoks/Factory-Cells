using DG.Tweening;
using Economics;
using UnityEngine;

namespace Cells.Object
{
    public class DroppedItem 
    {
        public DroppedItem(ItemStack itemStack, Vector2Int position) {
            ItemStack = itemStack;
            Position = position;
            VisualPosition = new Vector3(position.x, position.y);
        }
        
        public ItemStack ItemStack;
        public Vector2Int Position;
        public Vector3 VisualPosition;

        public void Animate(Vector3 from) {
            DOTween.Kill(this);
            DOTween.To(() => this.VisualPosition, x => this.VisualPosition = x, new Vector3(Position.x, Position.y), 0.35f)
                .SetEase(Ease.InOutSine)
                .SetId(this);
            VisualPosition = from;
        } 
    }
}