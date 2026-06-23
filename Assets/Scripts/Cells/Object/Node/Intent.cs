using Economics;

namespace Cells.Object.Node
{
    public class Intent
    {
        public Intent(IInventoryOut actor, IInventory victim) {
            Actor = actor;
            Victim = victim;
        }
        
        public IInventoryOut Actor;
        public IInventory Victim;
        public bool Activated;
        public bool Processed;

        // Returns the value depending on whether conveyors are cycling or not
        public bool Do() {
            if (Activated) return false;
            Processed = true;
            if (Victim.Intent is { Activated: false }) {
                if (Victim.Intent.Processed) {
                    var mvr = Actor.SuggestMoveStack();
                    Actor.CycleIntent = new CycleIntent(Actor.GetOutStack(), Actor, default);
                    Actor.CycleIntent.ItemStack = Actor.CycleIntent.ItemStack.Remove(mvr, out ItemStack removed);
                    Actor.CycleIntent.CauserStack = removed;
                    Activated = true;
                    return true;
                }

                if (Victim.Intent.Do()) {
                    Actor.CycleIntent = new CycleIntent(Actor.GetOutStack(), Victim.CycleIntent.Causer, Victim.CycleIntent.CauserStack);
                    {
                        var mv = Actor.SuggestMoveStack();
                        Victim.CycleIntent.ItemStack = Victim.CycleIntent.ItemStack.Add(mv, Victim.GetCapacity(), out ItemStack added);
                        Actor.CycleIntent.ItemStack = Actor.CycleIntent.ItemStack.Remove(added, out _);
                        Activated = true;
                    }

                    if (ReferenceEquals(Actor.CycleIntent.Causer.Intent.Victim, Actor)) {
                        var reserve = Actor.CycleIntent.CauserStack;
                        Actor.CycleIntent.ItemStack = Actor.CycleIntent.ItemStack.Add(reserve, Actor.GetCapacity(), out ItemStack mv);
                        var itemStackLeft = reserve.Remove(mv, out _);
                        Actor.CycleIntent.Causer.CycleIntent.ItemStack = Actor.CycleIntent.Causer.CycleIntent.ItemStack.Add(itemStackLeft, Actor.CycleIntent.Causer.GetCapacity() ,out ItemStack mv2);
                        itemStackLeft = itemStackLeft.Remove(mv2, out _);
                        if (itemStackLeft.IsEmpty()) {
                            IInventory cur = Actor;
                            do {
                                cur.SetItem(cur.CycleIntent.ItemStack);
                                cur = cur.Intent.Victim;
                            } while (cur != Actor);
                        } 
                        return false;
                    }  
                    
                    return true;
                }
            }

            {
                var mv = Actor.SuggestMoveStack();
                var realMv = Victim.AddItemStack(mv);
                Actor.RemoveItemStack(realMv);
                Activated = true;
            }

            return false;
        }
    }
}