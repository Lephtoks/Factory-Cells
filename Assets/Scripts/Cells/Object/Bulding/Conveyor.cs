using Cells.Object.Bulding.Mono;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Bulding
{
    public class Conveyor : OneSlotCellNode<ConveyorRepr, Conveyor>
    {
        public Conveyor(Cell parent, ConveyorRepr repr) : base(parent, repr) {
        }
        public override void SetItem(ItemStack itemStack) {
            base.SetItem(itemStack);
            LivingRepresentation.UpdateDisplay(this);
        }
    }
}