using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Building
{
    public class CellAnchor: Block, IRepresentable<DefaultRepr>
    {
        public override BlockType BlockType => BlockTypes.CELL_ANCHOR;
        public DefaultRepr LivingRepresentation { get; set; }
        
        public CellAnchor(Cell parent, Vector2Int pos) : base(parent, pos) {
        }

        public override void WhenBeingAddedToCell() {
            base.WhenBeingAddedToCell();
            Parent.locked = false;
        }

        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<DefaultRepr>) new CellAnchor(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y))).AssignRepresentation(repr);
        }
    }
}