using Economics;

namespace Cells.Object.Node
{
    public class Intent
    {
        public Intent(ICellNode actor, ICellNode victim, ItemStack backup) {
            Actor = actor;
            Victim = victim;
            Backup = backup;
        }
        
        public ICellNode Actor;
        public ICellNode Victim;
        public ItemStack Backup;
        public bool Activated;
        public bool Processed;

        public void Do() {
            if (Activated) return;
            Processed = true;
            if (Victim.GetIntent() is { Activated: false }) {
                if (Victim.GetIntent().Processed) {
                    var mvr = Actor.SuggestMoveStack();
                    Victim.SetReserveIntent(new ReserveIntent(Actor, Victim, mvr));
                    Actor.RemoveItemStack(mvr);
                    Activated = true;
                    return;
                }

                Victim.GetIntent().Do();
            }

            {
                var mv = Actor.SuggestMoveStack();
                var realMv = Victim.AddItemStack(mv);
                Actor.RemoveItemStack(realMv);
                Activated = true;
            }
    
            if (Actor.GetReserveIntent() != null) {
               var mv = Actor.AddItemStack(Actor.GetReserveIntent().Reserve);
               var mv2 = Actor.GetReserveIntent().Actor.AddItemStack(mv);
               if (!mv2.IsEmpty()) {
                   var cur = Actor;
                   do {
                       cur.SetItem(cur.GetIntent().Backup);
                       cur = cur.GetIntent().Victim;
                   } while (cur != Actor);
               }
            }
        }
    }
}