using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Conveyor : OneSlotBlock, IRepresentable<ConveyorRepr, Conveyor>, IDirected
    {
        public Direction Direction { get; }
        public ConveyorRepr Representation => AssetProvider.Instance.registry.conveyor;
        public ConveyorRepr LivingRepresentation { get; set; }
        
        public Conveyor(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos) {
            Direction = direction;
        }

        public override IEnumerable<Direction> OutDirections() {
            yield return Direction;
        }

        public override void SetItem(ItemStack itemStack) {
            base.SetItem(itemStack);
            LivingRepresentation.UpdateDisplay(this);
        }
        
        public static Conveyor Create(Cell parent, ConveyorRepr repr) {
            return ((IRepresentable<ConveyorRepr, Conveyor>)new Conveyor(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y), DirectionHelper.QuaternionToDirection(repr.transform.localRotation))).AssignRepresentation(repr);
        }
    }
}