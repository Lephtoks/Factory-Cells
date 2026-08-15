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
        }
        public bool AbleToMove(Vector2 a, Vector2 b)
        {
            Vector2 direction = b - a;

            int x = Mathf.FloorToInt(a.x);
            int y = Mathf.FloorToInt(a.y);

            int endX = Mathf.FloorToInt(b.x);
            int endY = Mathf.FloorToInt(b.y);

            if (!Cell.IsTileEmpty(new Vector2Int(x, y)))
                return false;

            if (direction == Vector2.zero)
                return true;

            int stepX = direction.x > 0 ? 1 : -1;
            int stepY = direction.y > 0 ? 1 : -1;

            float tDeltaX = direction.x == 0
                ? float.PositiveInfinity
                : Mathf.Abs(1f / direction.x);

            float tDeltaY = direction.y == 0
                ? float.PositiveInfinity
                : Mathf.Abs(1f / direction.y);

            float nextBoundaryX = direction.x > 0
                ? x + 1
                : x;

            float nextBoundaryY = direction.y > 0
                ? y + 1
                : y;

            float tMaxX = direction.x == 0
                ? float.PositiveInfinity
                : (nextBoundaryX - a.x) / direction.x;

            float tMaxY = direction.y == 0
                ? float.PositiveInfinity
                : (nextBoundaryY - a.y) / direction.y;

            while (x != endX || y != endY)
            {
                if (tMaxX < tMaxY)
                {
                    x += stepX;
                    tMaxX += tDeltaX;
                }
                else
                {
                    y += stepY;
                    tMaxY += tDeltaY;
                }

                if (!Cell.IsTileEmpty(new Vector2Int(x, y)))
                    return false;
            }

            return true;
        }
    }
}