using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Data;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Exporter : OneSlotBlock, IRepresentable<DefaultRepr>, IDirected
    {
        public override bool CanReceive => true;
        public override BlockType BlockType => BlockTypes.EXPORTER;
        public DefaultRepr LivingRepresentation { get; set; }
        public Direction Direction { get; }

        public Exporter(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos) {
            Direction = direction;
        }
        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<DefaultRepr>) new Exporter(parent, new Vector2Int(Mathf.FloorToInt(repr.transform.localPosition.x), Mathf.FloorToInt(repr.transform.localPosition.y)),  DirectionHelper.QuaternionToDirection(repr.transform.localRotation))).AssignRepresentation(repr);
        }

        public override void WhenBeingAddedToCell() {
            base.WhenBeingAddedToCell();
            Parent.Unlock(new ExporterLocker());
        }

        public override IEnumerable<Direction> OutDirections() {
            return base.OutDirections();
        }
    }
}