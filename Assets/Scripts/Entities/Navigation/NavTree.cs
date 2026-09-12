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
                RemoveNode(GetNavBlock(blockObject1.Position), Direction.SOUTH_WEST);
                RemoveNode(GetNavBlock(blockObject1.Position), Direction.SOUTH_EAST);
            }
            if (!Cell.TryGetObject(block.Position + Direction.EAST.ToVector2Int(), out var blockObject2)) flag += Direction.EAST;
            else {
                RemoveNode(GetNavBlock(blockObject2.Position), Direction.SOUTH_WEST);
                RemoveNode(GetNavBlock(blockObject2.Position), Direction.NORTH_WEST);
            }
            if (!Cell.TryGetObject(block.Position + Direction.SOUTH.ToVector2Int(), out var blockObject3)) flag += Direction.SOUTH;
            else {
                RemoveNode(GetNavBlock(blockObject3.Position), Direction.NORTH_EAST);
                RemoveNode(GetNavBlock(blockObject3.Position), Direction.NORTH_WEST);
            }
            if (!Cell.TryGetObject(block.Position + Direction.WEST.ToVector2Int(), out var blockObject4)) flag += Direction.WEST;
            else {
                RemoveNode(GetNavBlock(blockObject4.Position), Direction.NORTH_EAST);
                RemoveNode(GetNavBlock(blockObject4.Position), Direction.SOUTH_EAST);
            }
            if (!Cell.TryGetObject(block.Position + Direction.NORTH_EAST.ToVector2Int(), out var blockObject5)) flag += Direction.NORTH_EAST;
            else {
                RemoveNode(GetNavBlock(blockObject5.Position), Direction.SOUTH_WEST);
            }
            if (!Cell.TryGetObject(block.Position + Direction.SOUTH_EAST.ToVector2Int(), out var blockObject6)) flag += Direction.SOUTH_EAST;
            else {
                RemoveNode(GetNavBlock(blockObject6.Position), Direction.NORTH_WEST);
            }
            if (!Cell.TryGetObject(block.Position + Direction.SOUTH_WEST.ToVector2Int(), out var blockObject7)) flag += Direction.SOUTH_WEST;
            else {
                RemoveNode(GetNavBlock(blockObject7.Position), Direction.NORTH_EAST);
            }
            if (!Cell.TryGetObject(block.Position + Direction.NORTH_WEST.ToVector2Int(), out var blockObject8)) flag += Direction.NORTH_WEST;
            else {
                RemoveNode(GetNavBlock(blockObject8.Position), Direction.SOUTH_EAST);
            }
            
            if (flag.Contains(Direction.NORTH) && flag.Contains(Direction.EAST) && flag.Contains(Direction.NORTH_EAST)) BuildNode(block.Position, Direction.NORTH_EAST);
            if (flag.Contains(Direction.EAST) && flag.Contains(Direction.SOUTH) && flag.Contains(Direction.SOUTH_EAST)) BuildNode(block.Position, Direction.SOUTH_EAST);
            if (flag.Contains(Direction.WEST) && flag.Contains(Direction.NORTH) && flag.Contains(Direction.NORTH_WEST)) BuildNode(block.Position, Direction.NORTH_WEST);
            if (flag.Contains(Direction.WEST) && flag.Contains(Direction.SOUTH) && flag.Contains(Direction.SOUTH_WEST)) BuildNode(block.Position, Direction.SOUTH_WEST);
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

            DirectionFlag flag = new DirectionFlag();
            
            if (!Cell.TryGetObject(block.Position + Direction.EAST.ToVector2Int(), out var blockObject1)) flag += Direction.EAST;
            if (!Cell.TryGetObject(block.Position + Direction.NORTH.ToVector2Int(), out var blockObject2)) flag += Direction.NORTH;
            if (!Cell.TryGetObject(block.Position + Direction.WEST.ToVector2Int(), out var blockObject3)) flag += Direction.WEST;
            if (!Cell.TryGetObject(block.Position + Direction.SOUTH.ToVector2Int(), out var blockObject4)) flag += Direction.SOUTH;
            if (!Cell.TryGetObject(block.Position + Direction.NORTH_EAST.ToVector2Int(), out var blockObject5)) flag += Direction.NORTH_EAST;
            if (!Cell.TryGetObject(block.Position + Direction.SOUTH_EAST.ToVector2Int(), out var blockObject6)) flag += Direction.SOUTH_EAST;
            if (!Cell.TryGetObject(block.Position + Direction.SOUTH_WEST.ToVector2Int(), out var blockObject7)) flag += Direction.SOUTH_WEST;
            if (!Cell.TryGetObject(block.Position + Direction.NORTH_WEST.ToVector2Int(), out var blockObject8)) flag += Direction.NORTH_WEST;
            
            if (blockObject1 != null) { 
                if (flag.Contains(Direction.NORTH) && flag.Contains(Direction.NORTH_EAST)) BuildNode(blockObject1.Position, Direction.NORTH_WEST);
                if (flag.Contains(Direction.SOUTH) && flag.Contains(Direction.SOUTH_EAST)) BuildNode(blockObject1.Position, Direction.SOUTH_WEST);
            }
            if (blockObject2 != null) { 
                if (flag.Contains(Direction.EAST) && flag.Contains(Direction.NORTH_EAST)) BuildNode(blockObject2.Position, Direction.SOUTH_EAST);
                if (flag.Contains(Direction.WEST) && flag.Contains(Direction.NORTH_WEST)) BuildNode(blockObject2.Position, Direction.SOUTH_WEST);
            }
            if (blockObject3 != null) { 
                if (flag.Contains(Direction.NORTH) && flag.Contains(Direction.NORTH_WEST)) BuildNode(blockObject3.Position, Direction.NORTH_EAST);
                if (flag.Contains(Direction.SOUTH) && flag.Contains(Direction.SOUTH_WEST)) BuildNode(blockObject3.Position, Direction.SOUTH_EAST);
            }
            if (blockObject4 != null) { 
                if (flag.Contains(Direction.EAST) && flag.Contains(Direction.SOUTH_EAST)) BuildNode(blockObject4.Position, Direction.NORTH_EAST);
                if (flag.Contains(Direction.WEST) && flag.Contains(Direction.SOUTH_WEST)) BuildNode(blockObject4.Position, Direction.NORTH_WEST);
            }
            if (blockObject5 != null) { 
                if (flag.Contains(Direction.NORTH) && flag.Contains(Direction.EAST)) BuildNode(blockObject5.Position, Direction.SOUTH_WEST);
            }
            if (blockObject6 != null) { 
                if (flag.Contains(Direction.SOUTH) && flag.Contains(Direction.EAST)) BuildNode(blockObject6.Position, Direction.NORTH_WEST);
            }
            if (blockObject7 != null) { 
                if (flag.Contains(Direction.SOUTH) && flag.Contains(Direction.WEST)) BuildNode(blockObject7.Position, Direction.NORTH_EAST);
            }
            if (blockObject8 != null) { 
                if (flag.Contains(Direction.NORTH) && flag.Contains(Direction.WEST)) BuildNode(blockObject8.Position, Direction.SOUTH_EAST);
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

        private NavNode BuildNode(Vector2Int position, Direction direction) {
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

        private void RemoveNode(NavBlock block, Direction dir) {
            var node = block.Remove(dir);
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

        public NavNode Remove(Direction dir) {
            NavNode node;
            switch (dir) {
                case Direction.NORTH_EAST:
                    node = NorthEast;
                    NorthEast = null;
                    break;
                case Direction.SOUTH_EAST:
                    node = SouthEast;
                    SouthEast = null;
                    break;
                case Direction.NORTH_WEST:
                    node = NorthWest;
                    NorthWest = null;
                    break;
                case Direction.SOUTH_WEST:
                    node = SouthWest;
                    SouthWest = null;
                    break;
                default:
                    throw new ArgumentException("Invalid direction");
            }

            return node;
        }

        public void Set(NavNode node) {
            switch (node.Direction) {
                case Direction.NORTH_EAST:
                    NorthEast = node;
                    break;
                case Direction.SOUTH_EAST:
                    SouthEast = node;
                    break;
                case Direction.NORTH_WEST:
                    NorthWest = node;
                    break;
                case Direction.SOUTH_WEST:
                    SouthWest = node;
                    break;
            }
        }

        public IEnumerable<NavNode> GetSideNodes(Direction sideDirection) {
            switch (sideDirection) {
                case Direction.NORTH:
                    yield return NorthEast;
                    yield return NorthWest;
                    break;
                case Direction.EAST:
                    yield return NorthEast;
                    yield return SouthEast;
                    break;
                case Direction.WEST:
                    yield return SouthWest;
                    yield return NorthWest;
                    break;
                case Direction.SOUTH:
                    yield return SouthEast;
                    yield return SouthWest;
                    break;
                default:
                    throw new ArgumentException("Invalid direction");
            }
        }
    }
}