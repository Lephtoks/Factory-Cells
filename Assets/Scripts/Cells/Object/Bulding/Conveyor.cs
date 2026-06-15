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
        public static Conveyor Create(Cell parent, ConveyorRepr repr) {
            return new Conveyor(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y), DirectionHelper.Vector2Direction(repr.transform.localRotation * Vector3.forward)).AssignRepresentation(repr);
        }
        public override ConveyorRepr Representation => AssetProvider.Instance.registry.conveyor;
        public override void SetItem(ItemStack itemStack) {
            base.SetItem(itemStack);
            LivingRepresentation.UpdateDisplay(this);
        }
    }
}