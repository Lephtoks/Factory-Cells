using System;
using System.Collections.Generic;
using System.Linq;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Conveyor : OneSlotBlock, IRepresentable<ConveyorRepr>, IDirected, IItemDisplayable, IBlockUpdatable
    {
        public Direction Direction { get; }
        public override BlockType BlockType => BlockTypes.CONVEYOR;
        public ConveyorRepr LivingRepresentation { get; set; }
        public DroppedItem DroppedItem { get; set; }
        
        public Conveyor(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos) {
            Direction = direction;
        }

        public override IEnumerable<Direction> OutDirections() {
            yield return Direction;
        }
        
        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<ConveyorRepr>)new Conveyor(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y), DirectionHelper.QuaternionToDirection(repr.transform.localRotation))).AssignRepresentation(repr);
        }

        public override void IntentSucceed() {
            Debug.Log("Conveyor intent succeed");
        }

        public bool BlockUpdate() {
            if (Destroyed) return true;
            if (!LivingRepresentation) return false;
            var currentConnections = LivingRepresentation.Connections;
            LivingRepresentation.Connections = new DirectionFlag();
            foreach (var dir in (Direction[])Enum.GetValues(typeof(Direction))) {
                if (((ILookup)this).TryGetNeighbor(dir, out Block block)) {
                    if (dir == Direction) {
                        LivingRepresentation.Connections += Direction;
                    } else if (block is IInventoryOut inventoryOut && inventoryOut.OutDirections().Contains(dir.Opposite())) {
                        LivingRepresentation.Connections += dir;
                    }
                };
            }
            LivingRepresentation.UpdateConveyorDisplay();
            return currentConnections != LivingRepresentation.Connections;
        }
    }
}