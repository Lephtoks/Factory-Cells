namespace Cells.Object
{
    public interface ICellPlaceable
    {
        public Cell Parent { get; set; }
    }

    public static class ICellPlaceableExtensions
    {
        public static void SetParent(this Block block, Cell parent) {
            block.Parent?.RemoveBlockRepr(block);
            block.Parent = parent;
            parent.AddBlockRepr(block);
        }
    }
}