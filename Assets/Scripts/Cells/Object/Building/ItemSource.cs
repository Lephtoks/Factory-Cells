using System.Collections.Generic;
using Data;
using Economics;

namespace Cells.Object.Building
{
    public class ItemSource: OneSlotBlock, ISquareShadow
    {
        public override bool CanReceive => false;
        public override BlockType BlockType => BlockTypes.ITEM_SOURCE;
        
        public ItemSource() : base() {
        }
        
        
        public override IEnumerable<Direction> OutDirections() {
            yield return Direction.EAST;
            yield return Direction.WEST;
            yield return Direction.NORTH;
            yield return Direction.SOUTH;
        }
        
        public override void UpdateMove() {
            SetItem(Currency.STONE.OfCount(1));
        }
    }
}