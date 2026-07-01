namespace Cells.Object
{
    public interface IItemDisplayable : ICellPlaceable, IPositioned
    {
        DroppedItem DroppedItem { get; set; }

        void BindDrop(DroppedItem droppedItem) {
            if (droppedItem != null) droppedItem.Position = Position;
            if (DroppedItem != null) {
                Parent.BindTempDrop(droppedItem);
                return;
            }
            DroppedItem = droppedItem;
        }

        void RemoveDrop() {
            DroppedItem = null;
        }
    }
}