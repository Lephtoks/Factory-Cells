using System;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object
{
    public abstract class CellObject
    {
        public readonly Cell Parent;
        public Vector2Int Position { get; }

        public CellObject(Cell parent, Vector2Int pos) {
            Position = pos;
            Parent = parent;
        }
        public virtual void UpdateMove() {}
        
        public virtual void WhenBeingAddedToCell() {
        }

        public abstract ItemStack[] GetItems();

    }
    public abstract class CellObject<T, K> : CellObject where T : CellNodeRepr<K> where K : CellObject
    {
        public virtual T Representation => null;
        public T LivingRepresentation;

        public CellObject(Cell parent, Vector2Int pos) : base(parent, pos) {
        }

        public K AssignRepresentation(T repr) {
            this.LivingRepresentation = repr;
            var cellObject = this as K;
            repr.Init(cellObject);
            return cellObject;
        }
        
        public void CreateRepresentation() {
            // if (LivingRepresentation) UnityEngine.Object.Destroy(LivingRepresentation);
            // LivingRepresentation = UnityEngine.Object.Instantiate(Representation);
        }

        public bool TryGetNeighbor(Vector2Int direction, out CellObject neighbor) {
            return Parent.TryGetObject(Position + direction, out neighbor);
        }

        public bool TryGetNeighbor(Direction direction, out CellObject neighbor) {
            return TryGetNeighbor(direction.ToVector2Int(), out neighbor);
        }

        public override void WhenBeingAddedToCell() {
            if (this is not K k) throw new ArgumentException("Type argument mismatch");
            // CreateRepresentation();
            // LivingRepresentation.Init(k);
        }
    }
}