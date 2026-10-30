using System;
using Data;
using Data.GameManagement;
using Economics;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Exporter : Block, IInventory, IDirected
    {
        bool IInventory.CanReceive => true;

        public override BlockType BlockType => BlockTypes.EXPORTER;
        public Direction Direction { get; set; }

        public Exporter() : base() {
            Direction = Direction.NORTH;
        }

        public override void Init(Vector2Int pos) {
            base.Init(pos);
            Parent.Unlock(new ExporterLocker());
        }
        public ItemStack AddItemStack(ItemStack stack) {
            Debug.Log($"Added {stack.Count}: {stack.CurrencyType}");
            GameStorage.Instance.CurrencyData.Add(stack.CurrencyType, stack.Count);
            return stack;
        }

        public ItemStack RemoveItemStack(ItemStack stack) {
            throw new System.NotImplementedException("Impossible to remove items from exporter!");
        }

        public void SetItem(ItemStack itemStack) {
            AddItemStack(itemStack);
        }

        public ItemStack[] GetItems() {
            return Array.Empty<ItemStack>();
        }

        public int GetCapacity() {
            return int.MaxValue;
        }
    }
}