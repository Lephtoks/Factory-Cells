using System;
using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Node;
using Data.GameManagement;
using DG.Tweening;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private readonly Dictionary<Vector2Int, Block> _cellObjects = new();

        [NonSerialized] public float SynchronousConveyorTime;
        
        public void RemoveObject(Vector2Int position) {
            var block = _cellObjects[position];
            _cellObjects.Remove(position);
            if (block is IRepresentable representable) {
                Destroy((representable.LivingRepresentationObj as MonoBehaviour)?.gameObject);
                Debug.Log(block);
            }
        }

        public bool TryAddObject(Block block) {
            if (!IsTileEmpty(block.Position)) return false;
            _cellObjects.Add(block.Position, block);

            block.WhenBeingAddedToCell();
            return true;
        }
        public bool TryGetObject(Vector2Int position, out Block block) {
            return _cellObjects.TryGetValue(position, out block);
        }

        public bool IsTileEmpty(Vector2Int position) {
            return !_cellObjects.ContainsKey(position);
        }

        public void UpdatePreMove() {
            ResetWind();
            foreach (Block cellObject in _cellObjects.Values) {
                if (cellObject is IPreUpdatable node) {
                    node.UpdatePreMove();
                }
            }
            GameStorage.Instance.CurrencyData.Wind += GetWind();
        }

        public void UpdateMove() {
            var intents = new List<Intent>();
            foreach (Block cellObject in _cellObjects.Values) {
                if (cellObject is IInventory node) {
                    node.ResetIntent();
                    node.GenerateIntent();
                    if (node.Intent != null) intents.Add(node.Intent);
                }
            }

            _tempDroppedItems.Clear();
            foreach (Intent intent in intents) {
                intent.Do();
            }
            foreach (Block cellObject in _cellObjects.Values) {
                cellObject.UpdateMove();
            }

            SynchronousConveyorTime = 0;
            DOTween.Kill(this);
            DOTween.To(() => this.SynchronousConveyorTime, value => SynchronousConveyorTime = value, 1f, 0.25f)
                .SetEase(Ease.InOutSine)
                .SetId(this);
        }
    }
}