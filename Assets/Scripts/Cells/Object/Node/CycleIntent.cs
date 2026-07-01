using Economics;

namespace Cells.Object.Node
{
    public class CycleIntent
    {
        public CycleIntent(ItemStack stack, IInventoryOut causer, ItemStack causerStack) {
            ItemStack = stack;
            Causer = causer;
            CauserStack = causerStack;
        }
        
        public ItemStack ItemStack;
        public IInventoryOut Causer;
        public ItemStack CauserStack;
        public bool RemoveDrop;
        public DroppedItem Drop;
    }
}