using System.Collections.Generic;
using Cells.Object;
using Core;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private readonly List<DroppedItem> _tempDroppedItems = new();
        private static MaterialPropertyBlock _droppedItemBlock;
        
        private IEnumerable<DroppedItem> AllItems() {
            foreach (var item in _cellObjects.Values) {
                if (item is IItemDisplayable itemDisplayable && itemDisplayable.DroppedItem != null) yield return itemDisplayable.DroppedItem;
            }

            foreach (var item in _tempDroppedItems) {
                yield return item;
            }
        }
        
        private void DrawDroppedItems() {
            var rp = new RenderParams(AssetProvider.Instance.registry.render.ItemDropMaterial) {
                // layer = gameObject.layer
            };

            foreach (var item in AllItems()) {
                var settings = AssetProvider.Instance.GetCurrency(item.ItemStack.CurrencyType);
                if (settings == null || settings.icon == null) continue;

                _droppedItemBlock.SetTexture("_MainTex", settings.icon.texture);
                rp.matProps = _droppedItemBlock;

                var worldPos = CellPivot.TransformPoint(item.VisualPosition + new Vector3(0.5f + 1/32f, 0.5f + 1/32f, -0.5f));
                var matrix = Matrix4x4.TRS(worldPos, CellPivot.rotation, CellPivot.lossyScale * 0.8f);

                Graphics.RenderMesh(rp, AssetProvider.Instance.registry.render.ItemDropMesh, 0, matrix);
            }
        }

        public void BindTempDrop(DroppedItem droppedItem) {
            _tempDroppedItems.Add(droppedItem);
        }
    }
}