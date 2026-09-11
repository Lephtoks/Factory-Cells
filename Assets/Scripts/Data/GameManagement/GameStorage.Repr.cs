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
        private readonly Dictionary<BlockType, List<BlockRepr>> _reprCache = new();
        public readonly List<Representer> MultipleBlockBuilderList = new List<Representer>();
        private readonly List<Representer> _representerCache = new List<Representer>();

        public void DisposeRepresenterCache() {
            foreach (var  representer in _representerCache) {
                representer.ClearAndCache();
            }
            _representerCache.Clear();
        }

        public void RemoveRepresenter(Representer representer) {
            MultipleBlockBuilderList.Remove(representer);
            if (_representerCache.Count < 5) {
                _representerCache.Add(representer);
                representer.Displace();
                return;
            }
            
            representer.ClearAndCache();
        }

        public Representer AddRepresenter() {
            if (_representerCache.Count > 0) {
                var repr = _representerCache[^1];
                _representerCache.RemoveAt(_representerCache.Count - 1);
                MultipleBlockBuilderList.Add(repr);
                return repr;
            }
            var rep = Representer.Clone();
            MultipleBlockBuilderList.Add(rep);
            return rep;
        }


        public void SetAmountOfRepresenters(int amount) {
            if (MultipleBlockBuilderList.Count < amount) {
                for (int i = 0; i < amount - MultipleBlockBuilderList.Count; i++) {
                    AddRepresenter();
                }
            } else if (MultipleBlockBuilderList.Count > amount) {
                for (int i = MultipleBlockBuilderList.Count - 1; i >= amount; i--) {
                    RemoveRepresenter(MultipleBlockBuilderList[i]);
                }
            }
        }
        
        
        
        public BlockRepr CreateRepresentationCached(BlockRepr cellObjectRepresentation) {
            BlockRepr repr;
            if (_reprCache.TryGetValue(cellObjectRepresentation.BlockType, out List<BlockRepr> reprs)) {
                if (reprs.Count > 0) {
                    repr = reprs[^1];
                    reprs.RemoveAt(reprs.Count - 1);
                    return repr;
                }
            }
            else {
                _reprCache[cellObjectRepresentation.BlockType] = new List<BlockRepr>();
            }
            repr = Object.Instantiate(cellObjectRepresentation);
            repr.BlockType = cellObjectRepresentation.BlockType;
            return repr;
        }

        public void RemoveRepresentationCached(BlockRepr cellObjectRepresentation) {
            
            if (!_reprCache.TryGetValue(cellObjectRepresentation.BlockType, out List<BlockRepr> reprs)) {
                _reprCache[cellObjectRepresentation.BlockType] = reprs = new List<BlockRepr>();
            }
            if (reprs.Count < 5) {
                reprs.Add(cellObjectRepresentation);
                cellObjectRepresentation.MakeInvisible();
                return;
            }

            RemoveRepresentationCacheless(cellObjectRepresentation);

        }

        public void RemoveRepresentationCacheless(BlockRepr cellObjectRepresentation) {
            Object.Destroy(cellObjectRepresentation.gameObject);
            
        }
    }
}