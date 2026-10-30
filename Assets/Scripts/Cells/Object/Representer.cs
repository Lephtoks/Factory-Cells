using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.GameManagement;
using JetBrains.Annotations;
using UnityEngine;

namespace Cells.Object
{
    public class Representer
    {
        private readonly Dictionary<Vector2Int, Block> _reprs = new();
        [CanBeNull] public Cell CurrentCell { get; private set; }
        
        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool IsSmall() {
            return _reprs.Count == 1;
        }

        private void UpdateScales() {
            Width = _reprs.Count == 0
                ? 0
                : _reprs.Keys.Max(p => p.x) - _reprs.Keys.Min(p => p.x) + 1;
            Height = _reprs.Count == 0
                ? 0
                : _reprs.Keys.Max(p => p.y) - _reprs.Keys.Min(p => p.y) + 1;
        }
        
        public void Place(Vector2Int cellPos, Cell cell) {
            CurrentCell = cell;
            foreach (var blockRepr in _reprs) {
                Vector2Int pos;
                if (blockRepr.Value.BlockType != BlockTypes.EXPORTER)
                    pos = cellPos + blockRepr.Key.Rotate(GameStorage.Instance.RepresentationSettings.Direction);
                else {
                    pos = cell.NearestCellEdge(cell.CellPivot.InverseTransformPoint(MainController.Instance.CellBehaviourArguments.WorldPos), out Direction direction);
                    // TODO: blockRepr.Value.transform.localRotation = direction.Opposite().ToQuaternion();
                }
                if (cell.IsTileExist(pos) || blockRepr.Value.BlockType == BlockTypes.EXPORTER) {
                    // TODO: Non‑obvious behaviour
                    blockRepr.Value.SetPosition(pos); // Here
                    blockRepr.Value.SetParent(cell); // Here
                    
                    // TODO: blockRepr.Value.UseSettings(GameStorage.Instance.RepresentationSettings);
                    // if (cell.IsTileOccupied(pos)) {
                    //     blockRepr.Value.MakeWrong();
                    // }
                    // else {
                    //     blockRepr.Value.MakePhantom();
                    // }
                }
                else {
                    blockRepr.Value.SetParent(null);
                    // blockRepr.Value.MakeInvisible();
                }
            }
            
        }

        public void Displace() {
            foreach (var blockRepr in _reprs) {
                blockRepr.Value.SetParent(null);
                // blockRepr.Value.MakeInvisible();
            }

            CurrentCell = null;
        }

        public List<Block> Build() {
            if (CurrentCell == null) return new List<Block>();
            var added = new List<Block>();
            foreach (var repr in _reprs) {
                var blockType = repr.Value.BlockType;
                if (repr.Value.Parent != CurrentCell) {
                    GameStorage.Instance.RemoveRepresentationCached(repr.Value);
                    continue;
                }
                if (!CurrentCell.TryAddObject(repr.Value.Position, repr.Value)) {
                    GameStorage.Instance.RemoveRepresentationCacheless(repr.Value);
                }
                else {
                    added.Add(repr.Value);
                }

            }
            _reprs.Clear();

            return added;

        }

        public void SetRepresentation(BlockType blockType) {
            ClearAndCache();
            _reprs[Vector2Int.zero] = GameStorage.Instance.CreateRepresentationCached(blockType);
            GameStorage.Instance.DisposeRepresenterCache();
            UpdateScales();
        }

        public void SetRepresentationOfZone(Cell cell, Vector2Int start, Vector2Int end) {
            ClearAndCache();
            var min = Vector2Int.Min(start, end);
            var max = Vector2Int.Max(start, end);
            var anchor = (min + max) / 2;
            for (int x = min.x; x <= max.x; x++) {
                for (int y = min.y; y <= max.y; y++) {
                    var pos = new Vector2Int(x, y);
                    if (cell.TryGetObject(pos, out Block block)) {
                        _reprs[pos - anchor] = GameStorage.Instance.CreateRepresentationCached(block.BlockType);
                    }
                }
            }

            GameStorage.Instance.RepresentationSettings.Direction = Direction.EAST;
            GameStorage.Instance.DisposeRepresenterCache();
            UpdateScales();
        }

        public void SetCurrentBlockRepr() {
            BlockType activeBlock = GameStorage.Instance.BuildOption.GetActiveBlock();
            if (activeBlock == null) return;
            
            SetRepresentation(activeBlock);
        }

        public void ClearAndCache() {
            foreach (var blockRepr in _reprs.Values.ToArray()) {
                GameStorage.Instance.RemoveRepresentationCached(blockRepr);
            }
            _reprs.Clear();            
        }
        public Representer Clone() {
            var representer = new Representer();
            foreach (var pair in _reprs) 
            {
                representer._reprs[pair.Key] = GameStorage.Instance.CreateRepresentationCached(pair.Value.BlockType);
            }

            return representer;
        }
    }
}