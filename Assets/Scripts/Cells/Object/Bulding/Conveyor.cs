using Cells.Object.Bulding.Mono;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Bulding
{
    public class Conveyor : OneSlotCellNode<ConveyorRepr, Conveyor>
    {
        public Conveyor(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos, direction) {
        }

        public override ConveyorRepr Representation => BuildingInitializer.Instance.conveyor;
        public override void SetItem(ItemStack itemStack) {
            base.SetItem(itemStack);
            LivingRepresentation.UpdateDisplay(this);
        }
    }
}