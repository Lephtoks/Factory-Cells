using Cells.Object.Bulding.Mono;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Bulding
{
    public class Drill : OneSlotCellNode<DefaultRepr<Drill>, Drill>
    {

        public Drill(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos, direction) {
        }

        private int _counter = 0;
        public override void UpdateMove() {
            _counter++;
            if (_counter >= 5) {
                _counter = 0;
                AddItemStack(ItemTypes.COPPER.OfCount(1));
            }
        }
    }
}