using System;
using System.Collections.Generic;
using System.Linq;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using GameRenderer;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Conveyor : OneSlotBlock, IDirected, IBlockUpdatable, IKillable, IShadow
    {
        public float MaxHealth { get; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        private ShadowRect[] _shadows = ConveyorShadows.None;
        public override int GetCapacity() {
            return 1;
        }

        public Direction Direction { get; private set; }
        public override BlockType BlockType => BlockTypes.CONVEYOR;
        
        public Conveyor(Cell parent) : base(parent) {
            Direction = Direction.NORTH;
            MaxHealth = 100f;
            Health = MaxHealth;
            ShadowCount = 0;
        }

        public override IEnumerable<Direction> OutDirections() {
            yield return Direction;
        }
        
        public static Block Create(Cell parent) {
            return new Conveyor(parent);
        }

        public bool BlockUpdate() {
            if (Destroyed) return true;
            if (!LivingRepresentation) return false;
            var currentConnections = LivingRepresentation.Connections;
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
            LivingRepresentation.UpdateConveyorDisplay(LivingRepresentation.OriginalConveyor.Direction);
            return currentConnections != LivingRepresentation.Connections;
        }

        public IEnumerable<ShadowRect> CreateShadow() {
            return _shadows.Select((s) =>  new ShadowRect(Position + s.Position, s.Size, s.ShadowHeight));
        }
        public int ShadowCount {get; private set;}
        
        public void UpdateDisplayShadows() {
            Parent.RemoveShadows(this);
            _shadows = ConveyorShadows.GetShadows(LivingRepresentation.Connections, LivingRepresentation.Direction);
            ShadowCount = _shadows.Length;
            Parent.CreateShadows(this);
        }
    }

    internal static class ConveyorShadows
    {
        private const float U = 1 / 16f;
        private const float ShadowHeight = 0.25f;

        private static ShadowRect MakeRect(float x, float y, float width, float height) =>
            new ShadowRect(new Vector2(x, y), new Vector2(width, height), ShadowHeight);

        internal static readonly ShadowRect[] None = Array.Empty<ShadowRect>();

        // Одиночные направления.
        private static readonly ShadowRect EastOnly = MakeRect(3 * U, U, 13 * U, 13 * U);
        private static readonly ShadowRect WestOnly = MakeRect(0, U, 14 * U, 13 * U);
        private static readonly ShadowRect NorthOnly = MakeRect(3 * U, U, 11 * U, 15 * U);
        private static readonly ShadowRect SouthOnly = MakeRect(3 * U, 0, 11 * U, 14 * U);

        // Прямые участки (оба направления).
        private static readonly ShadowRect HorizontalRect = MakeRect(0, U, 1, 13 * U);   // E + W
        private static readonly ShadowRect VerticalRect = MakeRect(3 * U, 0, 11 * U, 1); // N + S

        private static readonly ShadowRect[] Horizontal = { HorizontalRect };
        private static readonly ShadowRect[] Vertical = { VerticalRect };

        // Повороты.
        private static readonly ShadowRect[] NorthEast = { NorthOnly, EastOnly };
        private static readonly ShadowRect[] EastSouth = { SouthOnly, EastOnly };
        private static readonly ShadowRect[] NorthWest = { NorthOnly, WestOnly };
        private static readonly ShadowRect[] SouthWest = { SouthOnly, WestOnly };

        // TurnBack / SideCombine (три направления).
        private static readonly ShadowRect[] NorthWithSides = { NorthOnly, HorizontalRect };
        private static readonly ShadowRect[] SouthWithSides = { SouthOnly, HorizontalRect };
        private static readonly ShadowRect[] WestWithSides = { VerticalRect, WestOnly };
        private static readonly ShadowRect[] EastWithSides = { VerticalRect, EastOnly };

        // Combine (четыре направления).
        private static readonly ShadowRect[] Cross = { VerticalRect, HorizontalRect };

        internal static ShadowRect[] GetShadows(DirectionFlag connections, Direction conveyorDirection) {
            switch (conveyorDirection) {
                case Direction.NORTH:
                    return connections.ToByte() switch {
                        4 or 5 => Vertical,
                        8 or 9 => NorthWest,
                        2 or 3 => NorthEast,
                        10 or 11 => NorthWithSides,
                        12 or 13 => WestWithSides,
                        6 or 7 => EastWithSides,
                        14 or 15 => Cross,
                        _ => Vertical
                    };
                case Direction.EAST:
                    return connections.ToByte() switch {
                        1 or 3 => NorthEast,
                        4 or 6 => EastSouth,
                        9 or 11 => NorthWithSides,
                        12 or 14 => SouthWithSides,
                        5 or 7 => EastWithSides,
                        13 or 15 => Cross,
                        _ => Horizontal
                    };
                case Direction.WEST:
                    return connections.ToByte() switch {
                        4 or 12 => SouthWest,
                        1 or 9 => NorthWest,
                        6 or 14 => SouthWithSides,
                        3 or 11 => NorthWithSides,
                        5 or 13 => WestWithSides,
                        7 or 15 => Cross,
                        _ => Horizontal
                    };
                case Direction.SOUTH:
                    return connections.ToByte() switch {
                        0 or 1 => Vertical,
                        2 or 6 => EastSouth,
                        8 or 12 => SouthWest,
                        3 or 7 => EastWithSides,
                        9 or 13 => WestWithSides,
                        10 or 14 => SouthWithSides,
                        11 or 15 => Cross,
                        _ => Vertical
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}