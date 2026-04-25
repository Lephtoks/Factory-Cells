using Cells.Object.Node;
using Economics;
using UnityEngine;

namespace Cells.Object
{
    public interface ICellNode
    {
        public void ResetIntent();
        public void GenerateIntent();
        public Intent GetIntent();
        public ReserveIntent GetReserveIntent();
        public void SetReserveIntent(ReserveIntent reserveIntent);
        
        public ItemStack SuggestMoveStack();
        public ItemStack AddItemStack(ItemStack stack);
        public ItemStack RemoveItemStack(ItemStack stack);
        public void SetItem(ItemStack itemStack);
    }

    public abstract class CellNode<T, K> : CellObject<T, K>, ICellNode where T : CellNodeRepr<K> where K : CellObject
    {
        public Intent Intent {get; protected set; }
        public ReserveIntent ReserveIntent { get; set; }
        public CellNode(Cell parent, Vector2Int pos) : base(parent, pos) {
        }

        public abstract void GenerateIntent();

        public void ResetIntent() {
            Intent = null;
            ReserveIntent = null;
        }

        public abstract ItemStack SuggestMoveStack();
        public abstract ItemStack AddItemStack(ItemStack stack);
        public abstract ItemStack RemoveItemStack(ItemStack stack);

        public abstract void SetItem(ItemStack itemStack);

        public Intent GetIntent() {
            return Intent;
        }

        public ReserveIntent GetReserveIntent() {
            return ReserveIntent;
        }

        public void SetReserveIntent(ReserveIntent reserveIntent) {
            ReserveIntent = reserveIntent;
        }
    }
}