using System;
using System.Collections.Generic;
using Cells.Object.Node;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object
{
    public abstract class OneSlotBlock : Block, IInventoryOut
    {
        private ItemStack _itemStack;
        public Intent Intent { get; set; }
        public CycleIntent CycleIntent { get; set; }

        public OneSlotBlock(Cell parent, Vector2Int pos) : base(parent, pos) {
        }

        public ItemStack[] GetItems() {
            return new[] {_itemStack};
        }

        public ItemStack GetOutStack() {
            return _itemStack;
        }

        public ItemStack SuggestMoveStack() {
            return _itemStack.OfCount(Math.Min(GetThroughput(), _itemStack.Count));
        }
        
        public ItemStack GetItemStack() {
            return _itemStack;
        }
        
        public ItemStack AddItemStack(ItemStack stack) {
            SetItem(_itemStack.Add(stack, GetCapacity(), out ItemStack added));
            return added;
        }

        public ItemStack RemoveItemStack(ItemStack stack) {
            SetItem(_itemStack.Remove(stack, out ItemStack removed));
            return removed;
        }

        public virtual void SetItem(ItemStack itemStack) {
            _itemStack = itemStack;
        }


        public virtual int GetCapacity() {
            return 999999;
        }

        public virtual int GetThroughput() {
            return 1;
        }

        public abstract IEnumerable<Direction> OutDirections();
        public virtual void IntentSucceed() {}
    }
}