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
                        var itemOut = Actor.GetOutStack();
                        Victim.CycleIntent.ItemStack = Victim.CycleIntent.ItemStack.Add(mv, Victim.GetCapacity(), out ItemStack added);
                        Actor.CycleIntent.ItemStack = Actor.CycleIntent.ItemStack.Remove(added, out _);
                        
                        
                        if (Victim is IItemDisplayable victimDisplayable) {
                            DroppedItem droppedItem;
                            if (Actor is IItemDisplayable actorDisplayable && itemOut.Count == added.Count) {
                                droppedItem = actorDisplayable.DroppedItem;
                                Actor.CycleIntent.RemoveDrop = true;
                            }
                            else {
                                droppedItem = new DroppedItem(added, victimDisplayable.Position);
                            }
                            Actor.CycleIntent.Drop = droppedItem;
                        }
                        
                        Activated = true;
                    }

                    if (ReferenceEquals(Actor.CycleIntent.Causer.Intent.Victim, Actor)) {
                        var reserve = Actor.CycleIntent.CauserStack;
                        Actor.CycleIntent.ItemStack = Actor.CycleIntent.ItemStack.Add(reserve, Actor.GetCapacity(), out ItemStack mv);
                        var itemStackLeft = reserve.Remove(mv, out _);
                        Actor.CycleIntent.Causer.CycleIntent.ItemStack = Actor.CycleIntent.Causer.CycleIntent.ItemStack.Add(itemStackLeft, Actor.CycleIntent.Causer.GetCapacity() ,out ItemStack mv2);
                        itemStackLeft = itemStackLeft.Remove(mv2, out _);
                        
                        if (Actor.CycleIntent.Causer is IItemDisplayable causerDisplayable && mv.Count == Actor.CycleIntent.Causer.GetOutStack().Count) {
                            Actor.CycleIntent.Causer.CycleIntent.Drop = causerDisplayable.DroppedItem;
                            Actor.CycleIntent.Causer.CycleIntent.RemoveDrop = true;
                        }
                        else {
                            if (Actor is IItemDisplayable actorDisplayable) {
                                Actor.CycleIntent.Causer.CycleIntent.Drop = new DroppedItem(mv, actorDisplayable.Position);
                            }
                        }
                        
                        if (itemStackLeft.IsEmpty()) {
                            
                            var inventories = new List<IInventory>();

                            IInventory cur = Actor.Intent.Victim;
                            do {
                                inventories.Add(cur);
                                cur = cur.Intent.Victim;
                                if (cur.CycleIntent.RemoveDrop && cur is IItemDisplayable victimDisplayable) {
                                    victimDisplayable.RemoveDrop();
                                }
                            } while (cur != Actor.Intent.Victim);


                            for (int i = inventories.Count - 1; i >= 0; i--) {
                                var inventory = inventories[i];
                                var sender = i-1 >= 0 ? (IInventoryOut)inventories[i-1] : (IInventoryOut)inventories[^1] ;

                                if (inventory is IItemDisplayable victimDisplayable) {
                                    if (sender.CycleIntent.Drop != null) {
                                        victimDisplayable.BindDrop(sender.CycleIntent.Drop);
                                        sender.CycleIntent.Drop.Animate(new Vector3(sender.Position.x, sender.Position.y));
                                    }
                                }

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
                if (Victim is IItemDisplayable victimDisplayable) {
                    DroppedItem droppedItem;
                    if (Actor is IItemDisplayable actorDisplayable && itemOut.Count == realMv.Count) {
                        droppedItem = actorDisplayable.DroppedItem;
                        actorDisplayable.RemoveDrop();
                    }
                    else {
                        droppedItem = new DroppedItem(realMv, victimDisplayable.Position);
                    }
                    victimDisplayable.BindDrop(droppedItem);
                    droppedItem.Animate(new Vector3(Actor.Position.x, Actor.Position.y));
                }
                Activated = true;
            }

            return false;
        }
    }
}