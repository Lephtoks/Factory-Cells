using Economics;

namespace Cells.Object.Node
{
    public class ReserveIntent
    {
        public ReserveIntent(ICellNode actor, ICellNode victim, ItemStack reserve) {
            Actor = actor;
            Victim = victim;
            Reserve = reserve;
        }
        
        public ICellNode Actor;
        public ICellNode Victim;
        public ItemStack Reserve;
    }
}