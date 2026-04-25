using Cells.Object.Bulding.Mono;
using Core;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Bulding
{
    public class Conveyor : OneSlotCellNode<ConveyorRepr, Conveyor>
    {
        public Conveyor(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos, direction) {
        }

        public override ConveyorRepr Representation => AssetProvider.Instance.registry.conveyor;
        public override void SetItem(ItemStack itemStack) {
            base.SetItem(itemStack);
            LivingRepresentation.UpdateDisplay(this);
        }
    }
}