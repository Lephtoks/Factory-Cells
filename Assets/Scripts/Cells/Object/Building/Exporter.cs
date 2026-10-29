using System;
using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Data;
using Data.GameManagement;
using Economics;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Exporter : Block, IInventory, IRepresentable<DefaultRepr>, IDirected
    {
        bool IInventory.CanReceive => true;

        public override BlockType BlockType => BlockTypes.EXPORTER;
        public DefaultRepr LivingRepresentation { get; set; }
        public Direction Direction { get; }

        public Exporter(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos) {
            Direction = direction;
        }
        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<DefaultRepr>) new Exporter(parent, new Vector2Int(Mathf.FloorToInt(repr.transform.localPosition.x), Mathf.FloorToInt(repr.transform.localPosition.y)),  DirectionHelper.QuaternionToDirection(repr.transform.localRotation))).AssignRepresentation(repr);
        }

        public override void Init() {
            base.Init();
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