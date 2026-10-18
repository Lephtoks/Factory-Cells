using Cells.Object.Building.Mono;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Importer : OneSlotBlock, IRepresentable<DefaultRepr>
    {
        public override bool CanReceive => false;
        public override BlockType BlockType => BlockTypes.IMPORTER;
        public DefaultRepr LivingRepresentation { get; set; }
        public Importer(Cell parent, Vector2Int pos) : base(parent, pos) {}
        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<DefaultRepr>) new Importer(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y))).AssignRepresentation(repr);
        }
    }
}