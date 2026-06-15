using System;
using Cells.Object.Node;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object
{
    public abstract class OneSlotCellNode<T, K> : DirectedCellNode<T, K> where T : CellNodeRepr<K> where K : CellObject
    {
        private ItemStack _itemStack;
        public OneSlotCellNode(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos, direction) {
        }

        public override ItemStack[] GetItems() {
            return new[] {_itemStack};
        }

        public override void GenerateIntent() {
            if (_itemStack.IsEmpty() || !TryGetNeighbor(GetDirection(), out CellObject neighbor) || neighbor is not ICellNode node) return;
            
            Intent = new Intent(this, node, _itemStack);
        }

        public override ItemStack SuggestMoveStack() {
            return _itemStack.OfCount(Math.Min(GetThroughput(), _itemStack.Count));
        }

        protected virtual int GetThroughput() {
            return 1;
        }

        public ItemStack GetItemStack() {
            return _itemStack;
        }

        public override ItemStack AddItemStack(ItemStack stack) {
            if (stack.IsEmpty()) return ItemStack.EMPTY;
            if (_itemStack.IsEmpty()) {
                int caped = Math.Min(stack.Count, GetCapacity());
                SetItem(stack.OfCount(caped));
                return _itemStack;
            }

            if (_itemStack.CurrencyType == stack.CurrencyType) {
                int newCount = _itemStack.Count + stack.Count;
                int caped = Math.Min(newCount, GetCapacity());
                int dif = caped - _itemStack.Count;
                SetItem(_itemStack.OfCount(caped));
                return _itemStack.OfCount(dif);
            }
            return ItemStack.EMPTY;
        }

        public override ItemStack RemoveItemStack(ItemStack stack) {
            if (stack.IsEmpty()) return ItemStack.EMPTY;
            if (_itemStack.IsEmpty()) return ItemStack.EMPTY;

            if (_itemStack.CurrencyType == stack.CurrencyType) {
                SetItem(_itemStack.OfCount(_itemStack.Count - stack.Count));
                return stack;
            }
            return ItemStack.EMPTY;

        }

        public override void SetItem(ItemStack itemStack) {
            _itemStack = itemStack;
        }

        protected int GetCapacity() {
            return 999999;
        }
    }
}