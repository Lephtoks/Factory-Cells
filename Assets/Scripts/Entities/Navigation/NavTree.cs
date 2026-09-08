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
                    if (Cell.AbleToMove(node.Position, second.Position)) {
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
                    
                    if (Cell.AbleToMove(node.Position, second.Position)) {
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
                if (Cell.AbleToMove(node.Position, child.Position)) {
                    Connect(node, child);
                }
            }
            Nodes.Add(node);
            return node;
        }
    }
}