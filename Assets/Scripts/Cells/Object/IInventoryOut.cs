using Cells.Object.Node;
using Economics;

namespace Cells.Object
{
    public interface IInventoryOut : IInventory, ILookup
    {
        ItemStack GetOutStack();
        ItemStack SuggestMoveStack();

        void IInventory.GenerateIntent() {
            ItemStack outStack = GetOutStack();
            if (outStack.IsEmpty() || !TryGetNeighbor(out Block neighbor) || neighbor is not IInventory node) return;
            
            Intent = new Intent(this, node, backup: outStack);
        }
    }
}