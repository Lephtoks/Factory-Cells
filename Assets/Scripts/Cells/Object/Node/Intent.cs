using System.Collections.Generic;
using Economics;
using UnityEngine;

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
            if (Victim is IInventoryOut { Intent: { Activated: false } } victimOut) {
                if (victimOut.Intent.Processed) {
                    var mvr = Actor.SuggestMoveStack();
                    Actor.CycleIntent = new CycleIntent(Actor.GetOutStack(), Actor, default);
                    Actor.CycleIntent.ItemStack = Actor.CycleIntent.ItemStack.Remove(mvr, out ItemStack removed);
                    Actor.CycleIntent.CauserStack = removed;
                    Activated = true;
                    return true;
                }

                if (victimOut.Intent.Do()) {
                    Actor.CycleIntent = new CycleIntent(Actor.GetOutStack(), victimOut.CycleIntent.Causer, victimOut.CycleIntent.CauserStack);
                    {
                        var mv = Actor.SuggestMoveStack();
                        var itemOut = Actor.GetOutStack();
                        victimOut.CycleIntent.ItemStack = victimOut.CycleIntent.ItemStack.Add(mv, Victim.GetCapacity(), out ItemStack added);
                        Actor.CycleIntent.ItemStack = Actor.CycleIntent.ItemStack.Remove(added, out _);
                        
                        Activated = true;
                    }

                    if (ReferenceEquals(Actor.CycleIntent.Causer.Intent.Victim, Actor)) {
                        var reserve = Actor.CycleIntent.CauserStack;
                        Actor.CycleIntent.ItemStack = Actor.CycleIntent.ItemStack.Add(reserve, Actor.GetCapacity(), out ItemStack mv);
                        var itemStackLeft = reserve.Remove(mv, out _);
                        Actor.CycleIntent.Causer.CycleIntent.ItemStack = Actor.CycleIntent.Causer.CycleIntent.ItemStack.Add(itemStackLeft, Actor.CycleIntent.Causer.GetCapacity() ,out ItemStack mv2);
                        itemStackLeft = itemStackLeft.Remove(mv2, out _);
                        
                        // if (Actor.CycleIntent.Causer is IItemDisplayable causerDisplayable && mv.Count == Actor.CycleIntent.Causer.GetOutStack().Count) {
                        //     Actor.CycleIntent.Causer.CycleIntent.Drop = causerDisplayable.DroppedItem;
                        //     Actor.CycleIntent.Causer.CycleIntent.RemoveDrop = true;
                        // }
                        // else {
                        //     if (Actor is IItemDisplayable actorDisplayable) {
                        //         Actor.CycleIntent.Causer.CycleIntent.Drop = new DroppedItem(mv, actorDisplayable.Position);
                        //     }
                        // }
                        
                        if (itemStackLeft.IsEmpty()) {
                            
                            var inventories = new List<IInventoryOut>();

                            IInventoryOut cur = (IInventoryOut) Actor.Intent.Victim;
                            do {
                                inventories.Add(cur);
                                cur = (IInventoryOut) cur.Intent.Victim;
                            } while (cur != Actor.Intent.Victim);


                            for (int i = inventories.Count - 1; i >= 0; i--) {
                                var inventory = inventories[i];
                                var sender = i-1 >= 0 ? inventories[i-1] : inventories[^1] ;

                                sender.Parent.AddDrop(new DroppedItem(sender.Position, inventory.Position));
                                

                                inventory.SetItem(inventory.CycleIntent.ItemStack);
                            }
                            
                        } 
                        return false;
                    }  
                    
                    return true;
                }
            }

            {
                var mv = Actor.SuggestMoveStack();
                var itemOut = Actor.GetOutStack();
                var realMv = Victim.AddItemStack(mv);
                Actor.RemoveItemStack(realMv);
                
                Actor.Parent.AddDrop(new DroppedItem(Actor.Position, ((IPositioned) Victim).Position));
                
                Activated = true;
            }

            return false;
        }
    }
}