using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Cells.Object;
using Data;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Entities.Navigation
{
    public class NavTree
    {
        private readonly Dictionary<Vector2Int, NavBlock> _navDictionary = new();
        public Cell Cell;
        private float radius = 0.2f;

        public NavTree(Cell cell) {
            this.Cell = cell;
        }

        public IEnumerable<NavNode> GetEnumerable() {
            foreach (var node in _navDictionary.Values) {
                if (node.NorthEast != null) yield return node.NorthEast;
                if (node.SouthEast != null) yield return node.SouthEast;
                if (node.SouthWest != null) yield return node.SouthWest;
                if (node.NorthWest != null) yield return node.NorthWest;
            }
        }

        public void RebuildWith(Block block) {
            if (block is not INavWall) return;
            foreach (var node in GetEnumerable()) {
                foreach (var second in GetEnumerable()) {
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

            DirectionFlag flag = new DirectionFlag();
            if (!Cell.TryGetObject(block.Position + Direction.NORTH.ToVector2Int(), out var blockObject1)) flag += Direction.NORTH;
            else {
                RemoveNode(GetNavBlock(blockObject1.Position), new DirectionFlag(6));
                RemoveNode(GetNavBlock(blockObject1.Position), new DirectionFlag(12));
            }
            if (!Cell.TryGetObject(block.Position + Direction.EAST.ToVector2Int(), out var blockObject2)) flag += Direction.EAST;
            else {
                RemoveNode(GetNavBlock(blockObject2.Position), new DirectionFlag(9));
                RemoveNode(GetNavBlock(blockObject2.Position), new DirectionFlag(12));
            }
            if (!Cell.TryGetObject(block.Position + Direction.SOUTH.ToVector2Int(), out var blockObject3)) flag += Direction.SOUTH;
            else {
                RemoveNode(GetNavBlock(blockObject3.Position), new DirectionFlag(3));
                RemoveNode(GetNavBlock(blockObject3.Position), new DirectionFlag(9));
            }
            if (!Cell.TryGetObject(block.Position + Direction.WEST.ToVector2Int(), out var blockObject4)) flag += Direction.WEST;
            else {
                RemoveNode(GetNavBlock(blockObject4.Position), new DirectionFlag(3));
                RemoveNode(GetNavBlock(blockObject4.Position), new DirectionFlag(6));
            }
            
            if (flag.Contains(Direction.NORTH) && flag.Contains(Direction.EAST)) BuildNode(block.Position, new DirectionFlag(3));
            if (flag.Contains(Direction.EAST) && flag.Contains(Direction.SOUTH)) BuildNode(block.Position, new DirectionFlag(6));
            if (flag.Contains(Direction.WEST) && flag.Contains(Direction.NORTH)) BuildNode(block.Position, new DirectionFlag(9));
            if (flag.Contains(Direction.WEST) && flag.Contains(Direction.SOUTH)) BuildNode(block.Position, new DirectionFlag(12));
        }

        public void RebuildWithout(Block block) {
            if (block is not INavWall) return;
            foreach (var node in GetEnumerable().ToArray()) {
                if (node.IntPosition == block.Position) {
                    RemoveNode(GetNavBlock(node.IntPosition), node.Direction);
                }

                foreach (var second in GetEnumerable()) {
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
            foreach (var child in GetEnumerable()) {
                if (Cell.AbleToMove(node.Position, child.Position)) {
                    Connect(node, child);
                }
            }
            GetNavBlock(position).Set(node);
            return node;
        }

        private void RemoveNode(NavBlock block, DirectionFlag flag) {
            var node = block.Remove(flag);
            if (node == null) return;
            foreach (var connected in node.Connections.Keys) {
                connected.Connections.Remove(node);
            }
            node.Connections.Clear();
        }

        private NavBlock GetNavBlock(Vector2Int position) {
            if (_navDictionary.TryGetValue(position, out var block)) {
                return block;
            }

            return _navDictionary[position] = new NavBlock();
        }
    }

    class NavBlock
    {
        [CanBeNull] public NavNode NorthEast;
        [CanBeNull] public NavNode SouthEast;
        [CanBeNull] public NavNode SouthWest;
        [CanBeNull] public NavNode NorthWest;

        public NavNode Remove(DirectionFlag flag) {
            NavNode node;
            switch (flag.ToByte()) {
                case 3:
                    node = NorthEast;
                    NorthEast = null;
                    break;
                case 6:
                    node = SouthEast;
                    SouthEast = null;
                    break;
                case 9:
                    node = NorthWest;
                    NorthWest = null;
                    break;
                case 12:
                    node = SouthWest;
                    SouthWest = null;
                    break;
                default:
                    throw new ArgumentException("Invalid direction");
            }

            return node;
        }

        public void Set(NavNode node) {
            switch (node.Direction.ToByte()) {
                case 3:
                    NorthEast = node;
                    break;
                case 6:
                    SouthEast = node;
                    break;
                case 9:
                    NorthWest = node;
                    break;
                case 12:
                    SouthWest = node;
                    break;
            }
        }
    }
}