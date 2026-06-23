using Cells.Object.Node;
using Economics;

namespace Cells.Object
{
    public interface IInventory
    {
        ItemStack AddItemStack(ItemStack stack);
        ItemStack RemoveItemStack(ItemStack stack);
        void SetItem(ItemStack itemStack);
        ItemStack[] GetItems();
        
        Intent Intent {get; set; }
        CycleIntent CycleIntent { get; set; }
        
        void GenerateIntent();
        
        void ResetIntent() {
            Intent = null;
            CycleIntent = null;
        }

        int GetThroughput();

        int GetCapacity();
    }
}