using Cells.Object.Node;
using Economics;

namespace Cells.Object
{
    public interface IInventory
    {
        bool CanReceive => true;
        ItemStack AddItemStack(ItemStack stack);
        ItemStack RemoveItemStack(ItemStack stack);
        void SetItem(ItemStack itemStack);
        ItemStack[] GetItems();
        int GetCapacity();
    }
}