using System.Collections.Generic;
using Cells;
using Cells.Object;
using Data;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Entities.Navigation
{
    public class NavTree
    {
        public List<NavNode> Nodes = new();
        public Cell Cell;
        private float radius = 0.2f;

        public NavTree(Cell cell) {
            this.Cell = cell;
        }

        public void RebuildWith(Block block) {
            if (block is not INavWall) return;
            foreach (var node in Nodes) {
                foreach (var second in Nodes) {
                    if (ReferenceEquals(node, second)) continue;
                    if (AbleToMove(node.Position, second.Position)) {
                        if (!node.Connections.ContainsKey(second)) {
                            Connect(node, second);
                        }
                    }
                    else {
                        if (node.Connections.Remove(second)) {
                            second.Connections.Remove(node);
                        }
                    }
                }
            }
            BuildNode(block.Position, new DirectionFlag(3));
            BuildNode(block.Position, new DirectionFlag(6));
            BuildNode(block.Position, new DirectionFlag(9));
            BuildNode(block.Position, new DirectionFlag(12));
        }

        public void RebuildWithout(Block block) {
            if (block is not INavWall) return;
            foreach (var node in Nodes.ToArray()) {
                if (node.IntPosition == block.Position) {
                    foreach (var child in node.Connections.Keys) {
                        child.Connections.Remove(node);
                    }
                    node.Connections.Clear();
                    Nodes.Remove(node);
                }

                foreach (var second in Nodes) {
                    if (ReferenceEquals(node, second)) continue;
                    if (node.IntPosition == block.Position) continue;
                    
                    if (AbleToMove(node.Position, second.Position)) {
                        if (!node.Connections.ContainsKey(second)) {
                            Connect(node, second);
                        }
                    }
                    else {
                        if (node.Connections.Remove(second)) {
                            second.Connections.Remove(node);
                        }
                    }
                }
            }
        }

        public List<NavNode> BuildPath(Vector2 a, Vector2 b) {
            return AStar.FindPath(this, a, b);
        }

        private void Connect(NavNode a, NavNode b) {
            float distance = Vector2.Distance(a.Position, b.Position);
            a.Connections.Add(b, distance);
            b.Connections.Add(a, distance);
        }

        private NavNode BuildNode(Vector2Int position, DirectionFlag direction) {
            var node = new NavNode();
            node.IntPosition = position;
            node.Direction = direction;
            node.Position = position + (Vector2)direction.ToVector2Int() * (radius+0.5f) + Vector2.one * 0.5f;
            foreach (var child in Nodes) {
                if (AbleToMove(node.Position, child.Position)) {
                    Connect(node, child);
                }
            }
            Nodes.Add(node);
            return node;
        }public bool AbleToMove(Vector2 a, Vector2 b)
        {
            Vector2 direction = b - a;

            int cellX = Mathf.FloorToInt(a.x);
            int cellY = Mathf.FloorToInt(a.y);

            if (!Cell.IsTileEmpty(new Vector2Int(cellX, cellY)))
                return false;

            if (direction == Vector2.zero)
                return true;

            int xDirection = direction.x > 0f ? 1 : direction.x < 0f ? -1 : 0;
            int yDirection = direction.y > 0f ? 1 : direction.y < 0f ? -1 : 0;

            float timePerXCell = direction.x != 0f
                ? Mathf.Abs(1f / direction.x)
                : float.PositiveInfinity;

            float timePerYCell = direction.y != 0f
                ? Mathf.Abs(1f / direction.y)
                : float.PositiveInfinity;

            float nextXBoundary = direction.x > 0f
                ? cellX + 1
                : cellX;

            float nextYBoundary = direction.y > 0f
                ? cellY + 1
                : cellY;

            float timeToNextXBoundary = direction.x != 0f
                ? (nextXBoundary - a.x) / direction.x
                : float.PositiveInfinity;

            float timeToNextYBoundary = direction.y != 0f
                ? (nextYBoundary - a.y) / direction.y
                : float.PositiveInfinity;

            while (true)
            {
                float timeToNextBoundary =
                    Mathf.Min(timeToNextXBoundary, timeToNextYBoundary);

                // Конец отрезка достигнут
                if (timeToNextBoundary > 1f)
                    break;

                if (timeToNextXBoundary < timeToNextYBoundary)
                {
                    cellX += xDirection;
                    timeToNextXBoundary += timePerXCell;
                }
                else if (timeToNextYBoundary < timeToNextXBoundary)
                {
                    cellY += yDirection;
                    timeToNextYBoundary += timePerYCell;
                }
                else
                {
                    // Ровно через угол клетки
                    cellX += xDirection;
                    cellY += yDirection;

                    timeToNextXBoundary += timePerXCell;
                    timeToNextYBoundary += timePerYCell;
                }

                if (!Cell.IsTileEmpty(new Vector2Int(cellX, cellY)))
                    return false;
            }

            return true;
        }
    }
}