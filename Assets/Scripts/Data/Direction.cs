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
        public static Direction Vector2Direction(Vector2 direction) {
            direction = direction.normalized;

            float up = Vector2.Dot(direction, Vector2.up);
            float right = Vector2.Dot(direction, Vector2.right);
            float down = Vector2.Dot(direction, Vector2.down);
            float left = Vector2.Dot(direction, Vector2.left);

            float max = Mathf.Max(up, right, down, left);

            if (Mathf.Approximately(max, up))
                return Direction.NORTH;

            if (Mathf.Approximately(max, right))
                return Direction.EAST;

            if (Mathf.Approximately(max, down))
                return Direction.SOUTH;

            return Direction.WEST;
        }
        public static Direction Vector2Direction(Vector2Int direction) {
            return Vector2Direction((Vector2)direction);
        }
        
        public static Quaternion ToQuaternion(this Direction direction)
        {
            return direction switch
            {
                Direction.NORTH => Quaternion.Euler(0, 0, 90),
                Direction.EAST  => Quaternion.Euler(0, 0, 0),
                Direction.SOUTH => Quaternion.Euler(0, 0, -90),
                Direction.WEST  => Quaternion.Euler(0, 0, 180),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction QuaternionToDirection(Quaternion rotation)
        {
            Vector2 dir = rotation * Vector2.right;
            return Vector2Direction(dir);
        }
    }
}