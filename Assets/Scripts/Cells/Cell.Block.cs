using System;
using System.Collections.Generic;
using System.Linq;
using Cells.Object;
using Cells.Object.Node;
using Core;
using Data;
using Data.GameManagement;
using DG.Tweening;
using Economics;
using Entities;
using Entities.Navigation;
using GameRenderer;
using Microsoft.CodeAnalysis;
using UnityEngine;
using IHealth = Cells.Object.IHealth;
using Random = UnityEngine.Random;

namespace Cells
{
    public partial class Cell
    {
        private readonly Dictionary<Vector2Int, Block> _cellObjects = new();

        [NonSerialized] public float SynchronousConveyorTime;

        public void RemoveObject(Vector2Int position, BlockRemovalType blockRemovalType = BlockRemovalType.DECONSTRUCTION) {
            var block = _cellObjects[position];
            block.Destroyed = true;
            _cellObjects.Remove(position);
            NavTree.RebuildWithout(block);
            block.OnDestroy();
            
            RemoveBlockRepr(block);

            if (block is IHealth health) {
                _healthBarRenderer.Remove(block);
            }
            RemoveShadows(block);

            if (blockRemovalType == BlockRemovalType.DESTRUCTION) { 
                _fragmentRenderer.Add(position);
            }
        }

        public void RemoveShadows(Block block) {
            if (block is not IShadow shadow) return;
            
            for (int i = 0; i < shadow.ShadowCount; i++) {
                _shadowRectRenderer.Remove(new ShadowID(block, i));
            }
        }

        public void CreateShadows(Block block) {
            if (block is not IShadow shadow) return;

            int i = 0;
            foreach (var rect in shadow.CreateShadow()) {
                _shadowRectRenderer.Add(new ShadowID(block, i), rect);
                i++;
            }
        }

        public bool TryAddObject(Vector2Int pos, Block block) {
            if (!IsTileEmpty(pos) && block.BlockType != BlockTypes.EXPORTER) return false;
            block.Init(pos);
            _cellObjects.Add(block.Position, block);
            NavTree.RebuildWith(block);
            CreateShadows(block);
            return true;
        }

        public void AddBlockRepr(Block block) {
            _blockRenderer.Add(block, new BlockRepresenter() {
                Position = block.Position,
                DefinitionIndex = AssetProvider.Instance.registry.render.GetDefinitionIndex(block.BlockType.Def.Animations[0])
            });
        } 

        public void RemoveBlockRepr(Block block) {
            _blockRenderer.Remove(block);
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
        public bool TryGetNavWall(Vector2Int position, out Block wall) {
            if (_cellObjects.TryGetValue(position, out Block block) && block is INavWall) {
                wall = block;
                return true;
            }

            wall = null;
            return false;
        }

        public bool IsTileEmpty(Vector2Int position) {
            return IsTileExist(position) && !IsTileOccupied(position);
        }

        public bool IsTileOccupied(Vector2Int position) {
            return _cellObjects.ContainsKey(position);
        }

        public bool IsTileExist(Vector2Int position) {
            return 0 <= position.x && position.x < size && 0 <= position.y && position.y < size;
        }

        public bool IsTileBlocksMovement(Vector2Int position) {
            return !IsTileEmpty(position) && TryGetObject(position, out Block block) && block is INavWall;
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
            UpdateDropPositions();
            var intents = new List<Intent>();
            foreach (Block cellObject in _cellObjects.Values) {
                if (cellObject is IInventoryOut node) {
                    node.ResetIntent();
                    node.GenerateIntent();
                    if (node.Intent != null) intents.Add(node.Intent);
                }
            }

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

        public Block NearestKillableBlock(Vector2 pos) {
            return _cellObjects.Values.SkipWhile((block) => block is not IKillable).OrderBy((block) => Vector2.Distance(block.Position, pos)).First();
        }

        public Vector2Int NearestCellEdge(Vector2 pos, out Direction normal) {
            var center = size / 2f;
            var dx = Math.Abs(pos.x - center);
            var dy = Math.Abs(pos.y - center);
            if (dx > dy) {
                normal = pos.x < center ? Direction.EAST : Direction.WEST;
                return new Vector2Int(pos.x < center ? -1 : size, Mathf.FloorToInt(pos.y));
            }
            normal = pos.y < center ? Direction.NORTH : Direction.SOUTH;
            return new Vector2Int(Mathf.FloorToInt(pos.x), pos.y < center ? -1 : size);
        }

        public bool TryAddOre(Vector2Int position, Currency currency) {
            if (!IsTileExist(position)) return false;
            _oreRenderer.Add(position, new OreInstance(position,  currency));
            return true;
        }

        public bool TryGetOre(Vector2Int position, out Currency currency) {
            currency = Currency.AIR;
            if (!IsTileExist(position)) return false;
            var res = _oreRenderer.TryGet(position, out var currencyInstance);
            currency = currencyInstance.OreType;
            return res;
        }

        public void RemoveOre(Vector2Int position) {
            if (!IsTileExist(position)) return;
            _oreRenderer.Remove(position);
        }

        private void GenerateOres(OreData[] ores) {
            foreach (var ore in ores) {
                // var veins = Mathf.RoundToInt(Random.Range(1, ore.OreAmount / 5 + 1));

                var left = ore.OreAmount;
                for (int i = 0; left > 0; i++) {
                    int amount = Mathf.Min(Mathf.RoundToInt(Random.Range(5, 15)), left);
                    if (!TryFindVeinSeed(out var seed)) break;
                    left -= GenerateVein(seed, amount, ore.OreType);
                }
            }
        }

        private static readonly Vector2Int[] VeinOffsets = {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        private const int VeinSeedAttempts = 16;

        private bool TryFindVeinSeed(out Vector2Int seed) {
            for (int attempt = 0; attempt < VeinSeedAttempts; attempt++) {
                var candidate = new Vector2Int(Random.Range(0, size), Random.Range(0, size));
                if (TryGetOre(candidate, out _)) continue;
                seed = candidate;
                return true;
            }

            for (int i = 0; i < size * size; i++) {
                var candidate = new Vector2Int(i % size, i / size);
                if (TryGetOre(candidate, out _)) continue;
                seed = candidate;
                return true;
            }

            seed = default;
            return false;
        }

        private int GenerateVein(Vector2Int seed, int amount, Currency currency) {
            if (amount <= 0) return 0;

            var candidates = new List<Vector2Int> { seed };
            var visited = new HashSet<Vector2Int> { seed };
            var placed = 0;

            while (placed < amount && candidates.Count > 0) {
                var index = Random.Range(0, candidates.Count);
                var current = candidates[index];
                candidates.RemoveAt(index);

                if (TryGetOre(current, out _)) {
                    EnqueueNeighbours(current);
                    continue;
                }

                if (!TryAddOre(current, currency)) continue;

                placed++;
                EnqueueNeighbours(current);

            }

            return placed;

            void EnqueueNeighbours(Vector2Int origin) {
                foreach (var offset in VeinOffsets) {
                    var neighbour = origin + offset;
                    if (IsTileExist(neighbour) && visited.Add(neighbour)) {
                        candidates.Add(neighbour);
                    }
                }
            }
        }
    }
}