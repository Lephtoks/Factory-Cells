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
        private readonly Dictionary<Vector2Int, BlockRepr> _reprs = new();
        [CanBeNull] public Cell CurrentCell { get; private set; }
        
        public int Width { get; private set; }
        public int Height { get; private set; }

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
                Vector2Int pos = cellPos + blockRepr.Key.Rotate(GameStorage.Instance.RepresentationSettings.Direction);
                if (cell.IsTileExist(pos)) {
                    blockRepr.Value.transform.SetParent(cell.CellPivot, false);
                    blockRepr.Value.transform.localPosition = new Vector3(pos.x + 0.5f, pos.y + 0.5f, -1);
                    blockRepr.Value.UseSettings(GameStorage.Instance.RepresentationSettings);
                    if (cell.IsTileOccupied(pos)) {
                        blockRepr.Value.MakeWrong();
                    }
                    else {
                        blockRepr.Value.MakePhantom();
                    }
                }
                else {
                    blockRepr.Value.transform.SetParent(null, false);
                    blockRepr.Value.MakeInvisible();
                }
            }
            
        }

        public void Displace() {
            foreach (var blockRepr in _reprs) {
                blockRepr.Value.transform.SetParent(null, false);
                blockRepr.Value.MakeInvisible();
            }

            CurrentCell = null;
        }

        public List<Block> Build() {
            if (CurrentCell == null) return new List<Block>();
            var added = new List<Block>();
            foreach (var repr in _reprs) {
                var blockType = repr.Value.BlockType;
                if (!repr.Value.transform.IsChildOf(CurrentCell.CellPivot)) {
                    GameStorage.Instance.RemoveRepresentationCached(repr.Value);
                    continue;
                }
                Block block = blockType.Create(CurrentCell, repr.Value);
                if (!CurrentCell.TryAddObject(block)) {
                    GameStorage.Instance.RemoveRepresentationCacheless(repr.Value);
                }
                else {
                    added.Add(block);
                }

            }
            _reprs.Clear();

            return added;

        }

        public void SetRepresentation(BlockRepr blockReprPrefab) {
            ClearAndCache();
            _reprs[Vector2Int.zero] = GameStorage.Instance.CreateRepresentationCached(blockReprPrefab);
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
                        _reprs[pos - anchor] = GameStorage.Instance.CreateRepresentationCached(block.BlockType.Def.Representation);
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
            
            SetRepresentation(activeBlock.Def.Representation);
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
                representer._reprs[pair.Key] = GameStorage.Instance.CreateRepresentationCached(pair.Value.BlockType.Def.Representation);
            }

            return representer;
        }
    }
}