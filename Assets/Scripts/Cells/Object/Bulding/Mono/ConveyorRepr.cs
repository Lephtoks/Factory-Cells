using Core;
using Data;
using UnityEngine;

namespace Cells.Object.Bulding.Mono
{
    public class ConveyorRepr : CellNodeRepr<Conveyor>
    {
        [SerializeField] private SpriteRenderer itemOnConveyor;
        public override void Init(Conveyor original) {
            MakeReal();
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -1) + new Vector3(0.5f, 0.5f, 0);
            var vector2Int = original.GetDirection().ToVector2Int();
            transform.localRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(vector2Int.x, vector2Int.y, 0));
            UpdateDisplay(original);
        }

        public void UpdateDisplay(Conveyor conveyor) {
            itemOnConveyor.sprite =
                AssetProvider.Instance.GetCurrency(conveyor.GetItemStack().CurrencyType).icon;
        }
    }
}