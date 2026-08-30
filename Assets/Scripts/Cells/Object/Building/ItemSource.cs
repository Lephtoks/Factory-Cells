using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Building
{
    public class ItemSource: OneSlotBlock, IRepresentable<DefaultRepr>
    {
        public override BlockType BlockType => BlockTypes.ITEM_SOURCE;
        public DefaultRepr LivingRepresentation { get; set; }
        
        public ItemSource(Cell parent, Vector2Int pos) : base(parent, pos) {
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

        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<DefaultRepr>) new ItemSource(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y))).AssignRepresentation(repr);
        }
    }
}