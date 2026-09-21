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
            if (outStack.IsEmpty() || !TryGetReceiver(out IInventory node)) return;
            
            Intent = new Intent(this, node);
        }
    }
}