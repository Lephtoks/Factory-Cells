using System;
using System.Collections.Generic;
using System.Linq;
using Data.GameManagement;
using JetBrains.Annotations;
using UnityEngine;

namespace Cells.Object
{
    public class Representer
    {
        private readonly Dictionary<Vector2Int, BlockRepr> _reprs = new();
        [CanBeNull] public Cell CurrentCell { get; private set; }
        
        public void Place(Vector2Int cellPos, Cell cell) {
            CurrentCell = cell;
            foreach (var blockRepr in _reprs) {
                Vector2Int pos = cellPos + blockRepr.Key;
                if (cell.IsTileEmpty(pos)) {
                    blockRepr.Value.transform.SetParent(cell.CellPivot, false);
                    blockRepr.Value.transform.localPosition = new Vector3(pos.x + 0.5f, pos.y + 0.5f, -1);
                    blockRepr.Value.MakePhantom();
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
                };
                Block block = blockType.Create(CurrentCell, repr.Value);
                if (!CurrentCell.TryAddObject(block)) {
                    GameStorage.Instance.RemoveRepresentationCached(repr.Value);
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
    }
}