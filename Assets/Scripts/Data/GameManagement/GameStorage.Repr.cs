using System;
using System.Collections.Generic;
using Cells.Object;
using Object = UnityEngine.Object;

namespace Data.GameManagement
{
    public partial class GameStorage
    {
        public readonly List<BlockRepr> NodeReprs = new();
        public readonly RepresentationSettings RepresentationSettings = new();
        private readonly Dictionary<System.Type, List<BlockRepr>> _reprCache = new();
        
        public void UpdatePointerRepr() {
            foreach (var repr in NodeReprs.ToArray()) {
                RemoveRepresentation(repr);
            }
            CreatePointerRepr();
            
        }

        public void CreatePointerRepr() {
            BlockRepr blockRepr = CreateRepresentation(ActiveCard.Block.Def.Representation);
            blockRepr.MakeInvisible();
        }
        public void SetAmountOfRepresentations(BlockRepr cellObjectRepresentation, int reprs) {
            int visibleCount = 0;
            System.Type targetType = cellObjectRepresentation.GetType();

            foreach (var repr in NodeReprs.ToArray()) {
                if (repr.GetType() != targetType) {
                    RemoveRepresentation(repr);
                    continue;
                }

                if (visibleCount < reprs) {
                    repr.MakePhantom();
                    repr.UseSettings(RepresentationSettings);
                    visibleCount++;
                }
                else {
                    RemoveRepresentation(repr);
                }
            }

            for (int i = visibleCount; i < reprs; i++) {
                var repr = CreateRepresentation(cellObjectRepresentation);
                repr.MakePhantom();
                repr.UseSettings(RepresentationSettings);
            }
        }

        public BlockRepr CreateRepresentation(BlockRepr cellObjectRepresentation) {
            BlockRepr repr;
            Type type = cellObjectRepresentation.GetType();
            if (_reprCache.TryGetValue(type, out List<BlockRepr> reprs)) {
                if (reprs.Count > 0) {
                    repr = reprs[^1];
                    reprs.RemoveAt(reprs.Count - 1);
                    NodeReprs.Add(repr);
                    return repr;
                }
            }
            else {
                _reprCache[type] = new List<BlockRepr>();
            }
            repr = Object.Instantiate(cellObjectRepresentation);
            NodeReprs.Add(repr);
            return repr;
        }

        public void RemoveRepresentation(BlockRepr cellObjectRepresentation) {
            
            Type type = cellObjectRepresentation.GetType();
            if (!_reprCache.TryGetValue(type, out List<BlockRepr> reprs)) {
                _reprCache[type] = reprs = new List<BlockRepr>();
            }
            NodeReprs.Remove(cellObjectRepresentation);
            if (reprs.Count < 5) {
                reprs.Add(cellObjectRepresentation);
                cellObjectRepresentation.MakeInvisible();
                return;
            }
            Object.Destroy(cellObjectRepresentation.gameObject);
            
        }
    }
}