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
    public class Conveyor : OneSlotBlock, IRepresentable<ConveyorRepr>, IDirected, IItemDisplayable, IBlockUpdatable, IKillable
    {
        public float MaxHealth { get; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public override int GetCapacity() {
            return 1;
        }

        public Direction Direction { get; }
        public override BlockType BlockType => BlockTypes.CONVEYOR;
        public ConveyorRepr LivingRepresentation { get; set; }
        public DroppedItem DroppedItem { get; set; }
        
        public Conveyor(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos) {
            Direction = direction;
            MaxHealth = 100f;
            Health = MaxHealth;
        }

        public override IEnumerable<Direction> OutDirections() {
            yield return Direction;
        }
        
        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<ConveyorRepr>)new Conveyor(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y), ((ConveyorRepr) repr).Direction)).AssignRepresentation(repr);
        }

        public bool BlockUpdate() {
            if (Destroyed) return true;
            if (!LivingRepresentation) return false;
            var currentConnections = LivingRepresentation.Connections;
            Debug.Log(currentConnections.ToByte());
            LivingRepresentation.Connections = new DirectionFlag();
            foreach (var dir in new[] { Direction.EAST, Direction.NORTH, Direction.WEST, Direction.SOUTH}) {
                if (((ILookup)this).TryGetNeighbor(dir, out Block block)) {
                    if (dir == Direction) {
                        LivingRepresentation.Connections += Direction;
                    } else if (block is IInventoryOut inventoryOut && inventoryOut.OutDirections().Contains(dir.Opposite())) {
                        LivingRepresentation.Connections += dir;
                    }
                };
            }
            Debug.Log(LivingRepresentation.Connections.ToByte());
            LivingRepresentation.UpdateConveyorDisplay(LivingRepresentation.OriginalConveyor.Direction);
            return currentConnections != LivingRepresentation.Connections;
        }
    }
}