using System;
using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Node;
using Data;
using Data.GameManagement;
using DG.Tweening;
using Entities;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private readonly Dictionary<Vector2Int, Block> _cellObjects = new();

        [NonSerialized] public float SynchronousConveyorTime;
        
        public void RemoveObject(Vector2Int position) {
            var block = _cellObjects[position];
            block.Destroyed = true;
            _cellObjects.Remove(position);
            NavTree.RebuildWithout(block);
            block.OnDestroy();
            if (block is IRepresentable representable) {
                Destroy((representable.LivingRepresentationObj as MonoBehaviour)?.gameObject);
            }
        }

        public bool TryAddObject(Block block) {
            if (!IsTileEmpty(block.Position)) return false;
            _cellObjects.Add(block.Position, block);
            NavTree.RebuildWith(block);

            block.WhenBeingAddedToCell();
            return true;
        }

        public void BlockUpdate(Block block) {
            Queue<IBlockUpdatable> queue = new();
            List<IBlockUpdatable> updated = new();
            if (block is IBlockUpdatable blockUpdatable) {
                queue.Enqueue(blockUpdatable);
            } else if (block is IInventoryOut inventoryOut) {
                if (TryGetObject(block.Position + Direction.NORTH.ToVector2Int(), out var blockObject1) && blockObject1 is IBlockUpdatable newBlockUpdatable1 && !updated.Contains(newBlockUpdatable1)) queue.Enqueue(newBlockUpdatable1);
                if (TryGetObject(block.Position + Direction.EAST.ToVector2Int(), out var blockObject2) && blockObject2 is IBlockUpdatable newBlockUpdatable2 && !updated.Contains(newBlockUpdatable2)) queue.Enqueue(newBlockUpdatable2);
                if (TryGetObject(block.Position + Direction.SOUTH.ToVector2Int(), out var blockObject3) && blockObject3 is IBlockUpdatable newBlockUpdatable3 && !updated.Contains(newBlockUpdatable3)) queue.Enqueue(newBlockUpdatable3);
                if (TryGetObject(block.Position + Direction.WEST.ToVector2Int(), out var blockObject4) && blockObject4 is IBlockUpdatable newBlockUpdatable4 && !updated.Contains(newBlockUpdatable4)) queue.Enqueue(newBlockUpdatable4);
            }
            while (queue.Count > 0) {
                var current = queue.Dequeue();
                var updateNeighbours = current.BlockUpdate();
                updated.Add(current);
                if (updateNeighbours) {
                    if (TryGetObject(((Block) current).Position + Direction.NORTH.ToVector2Int(), out var blockObject1) && blockObject1 is IBlockUpdatable newBlockUpdatable1 && !updated.Contains(newBlockUpdatable1)) queue.Enqueue(newBlockUpdatable1);
                    if (TryGetObject(((Block) current).Position + Direction.EAST.ToVector2Int(), out var blockObject2) && blockObject2 is IBlockUpdatable newBlockUpdatable2 && !updated.Contains(newBlockUpdatable2)) queue.Enqueue(newBlockUpdatable2);
                    if (TryGetObject(((Block) current).Position + Direction.SOUTH.ToVector2Int(), out var blockObject3) && blockObject3 is IBlockUpdatable newBlockUpdatable3 && !updated.Contains(newBlockUpdatable3)) queue.Enqueue(newBlockUpdatable3);
                    if (TryGetObject(((Block) current).Position + Direction.WEST.ToVector2Int(), out var blockObject4) && blockObject4 is IBlockUpdatable newBlockUpdatable4 && !updated.Contains(newBlockUpdatable4)) queue.Enqueue(newBlockUpdatable4);
                }
            }
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
            // TODO:
            // AddBullet(new Bullet(this, new Vector2(4, 4), 0f, BulletTypes.DEFAULT));
            // AddBullet(new Bullet(this, new Vector2(2, 6), -90f, BulletTypes.DEFAULT));
            
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

            SynchronousConveyorTime = 1/16f;
            DOTween.Kill(this);
            DOTween.To(() => this.SynchronousConveyorTime, value => SynchronousConveyorTime = value, 1f + 1/16f, 0.35f)
                .SetEase(Ease.InOutSine)
                .SetId(this);
        }
    }
}