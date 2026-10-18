using Cells.Object.Node;
using Economics;

namespace Cells.Object
{
    public interface IInventoryOut : IInventory, ILookup
    {
        ItemStack GetOutStack();
        ItemStack SuggestMoveStack();
        Intent Intent {get; set; }
        CycleIntent CycleIntent { get; set; }

        void GenerateIntent() {
            ItemStack outStack = GetOutStack();
            if (outStack.IsEmpty() || !TryGetReceiver(out IInventory node)) return;
            
            Intent = new Intent(this, node);
        }
        
        void ResetIntent() {
            Intent = null;
            CycleIntent = null;
        }
    }
}