using Economics;

namespace Cells.Object.Node
{
    public class Intent
    {
        public Intent(IInventoryOut actor, IInventory victim, ItemStack backup) {
            Actor = actor;
            Victim = victim;
            Backup = backup;
        }
        
        public IInventoryOut Actor;
        public IInventory Victim;
        public ItemStack Backup;
        public bool Activated;
        public bool Processed;

        public void Do() {
            if (Activated) return;
            Processed = true;
            if (Victim.Intent is { Activated: false }) {
                if (Victim.Intent.Processed) {
                    var mvr = Actor.SuggestMoveStack();
                    Victim.ReserveIntent = new ReserveIntent(Actor, Victim, mvr);
                    Actor.RemoveItemStack(mvr);
                    Activated = true;
                    return;
                }

                Victim.Intent.Do();
            }

            {
                var mv = Actor.SuggestMoveStack();
                var realMv = Victim.AddItemStack(mv);
                Actor.RemoveItemStack(realMv);
                Activated = true;
            }
    
            if (Actor.ReserveIntent != null) {
               var mv = Actor.AddItemStack(Actor.ReserveIntent.Reserve);
               var mv2 = Actor.ReserveIntent.Actor.AddItemStack(mv);
               if (!mv2.IsEmpty()) {
                   IInventory cur = Actor;
                   do {
                       cur.SetItem(cur.Intent.Backup);
                       cur = cur.Intent.Victim;
                   } while (cur != Actor);
               }
            }
        }
    }
}