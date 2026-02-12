using System;
using UnityEngine;

namespace Data
{
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    public static class DirectionHelper
    {
        public static Vector2Int ToVector2Int(this Direction direction) {
            return direction switch {
                Direction.NORTH => new Vector2Int(0, 1),
                Direction.EAST => new Vector2Int(1, 0),
                Direction.SOUTH => new Vector2Int(0, -1),
                Direction.WEST => new Vector2Int(-1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}