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

        public override void GenerateIntent() {
            // if (_itemStack.IsEmpty() || !TryGetNeighbor(GetDirection(), out CellObject neighbor) || neighbor is not CellNode<CellNodeRepr<object>> node) return;
            //
            // Intent = new Intent(this, node, _itemStack);
        }

        public override ItemStack SuggestMoveStack() {
            return _itemStack.OfCount(Math.Min(GetThroughput(), _itemStack.Count));
        }

        protected virtual int GetThroughput() {
            return 1;
        }

        public override ItemStack AddItemStack(ItemStack stack) {
            if (stack.IsEmpty()) return ItemStack.EMPTY;
            if (_itemStack.IsEmpty()) {
                int caped = Math.Min(stack.Count, GetCapacity());
                _itemStack = stack.OfCount(caped);
                return _itemStack;
            }

            if (_itemStack.Type == stack.Type) {
                int newCount = _itemStack.Count + stack.Count;
                int caped = Math.Min(newCount, GetCapacity());
                int dif = caped - _itemStack.Count;
                _itemStack = _itemStack.OfCount(caped);
                return _itemStack.OfCount(dif);
            }
            return ItemStack.EMPTY;
        }

        public override ItemStack RemoveItemStack(ItemStack stack) {
            if (stack.IsEmpty()) return ItemStack.EMPTY;
            if (_itemStack.IsEmpty()) return ItemStack.EMPTY;

            if (_itemStack.Type == stack.Type) {
                _itemStack = _itemStack.OfCount(_itemStack.Count - stack.Count);
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