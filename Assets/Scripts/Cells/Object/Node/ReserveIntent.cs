using Economics;

namespace Cells.Object.Node
{
    public class ReserveIntent
    {
        public ReserveIntent(IInventory actor, IInventory victim, ItemStack reserve) {
            Actor = actor;
            Victim = victim;
            Reserve = reserve;
        }
        
        public IInventory Actor;
        public IInventory Victim;
        public ItemStack Reserve;
    }
}