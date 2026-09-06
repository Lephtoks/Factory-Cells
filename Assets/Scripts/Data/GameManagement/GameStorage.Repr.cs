using System;
using System.Collections.Generic;
using Cells.Object;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data.GameManagement
{
    public partial class GameStorage
    {
        public readonly Representer Representer = new();
        public readonly RepresentationSettings RepresentationSettings = new();
        private readonly Dictionary<System.Type, List<BlockRepr>> _reprCache = new();
        
        public BlockRepr CreateRepresentationCached(BlockRepr cellObjectRepresentation) {
            BlockRepr repr;
            Type type = cellObjectRepresentation.GetType();
            if (_reprCache.TryGetValue(type, out List<BlockRepr> reprs)) {
                if (reprs.Count > 0) {
                    repr = reprs[^1];
                    reprs.RemoveAt(reprs.Count - 1);
                    return repr;
                }
            }
            else {
                _reprCache[type] = new List<BlockRepr>();
            }
            repr = Object.Instantiate(cellObjectRepresentation);
            repr.BlockType = cellObjectRepresentation.BlockType;
            return repr;
        }

        public void RemoveRepresentationCached(BlockRepr cellObjectRepresentation) {
            
            Type type = cellObjectRepresentation.GetType();
            if (!_reprCache.TryGetValue(type, out List<BlockRepr> reprs)) {
                _reprCache[type] = reprs = new List<BlockRepr>();
            }
            if (reprs.Count < 5) {
                reprs.Add(cellObjectRepresentation);
                cellObjectRepresentation.MakeInvisible();
                return;
            }
            Object.Destroy(cellObjectRepresentation.gameObject);
            
        }
    }
}