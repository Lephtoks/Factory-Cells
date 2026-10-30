using System;
using System.Collections.Generic;
using Attributes;
using Cells.Object;
using Core;
using Data;
using Data.GameManagement;
using DG.Tweening;
using Economics;
using Entities.Navigation;
using GameRenderer;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Cells
{
    public partial class Cell : MonoBehaviour
    {
        public Transform FramePivot { get; private set; }
        public Transform CellPivot { get; private set; }
        private Vector3 _baseScale;
        public UIGlow Glow;
        private bool _initialized;
        private ObjectIndirectRenderer<Block, Vector3> _healthBarRenderer; 
        private FragmentRenderer _fragmentRenderer; 
        private ObjectIndirectRenderer<ShadowID, ShadowRect> _shadowRectRenderer;
        private DropRenderer _dropRenderer;
        private OreRenderer _oreRenderer;
        private BlockRenderer _blockRenderer;

        public static Cell Create(Cell prefab, ICellBehaviour behaviour = null, CellArguments cellArgs = null) {
            var cell = Instantiate(prefab);
            cell.Init(behaviour, cellArgs);
            return cell;
        }
        private void Init(ICellBehaviour behaviour = null, CellArguments cellArgs = null) {
            _initialized = true;
            _behaviour = CellBehaviours.NONE;
            NavTree = new NavTree(this);

            // One instance slot per grid tile (+1 for exporters on the border), x2 as a safety
            // margin. Buffers used to default to 10000 entries each; with six buffers per cell
            // that was ~0.6 MB of GPU memory per cell, mostly unused.
            var tileCapacity = (size + 1) * (size + 1) * 2;

            _healthBarRenderer = new ObjectIndirectRenderer<Block, Vector3>(CellPivot.gameObject,
                AssetProvider.Instance.registry.render.HealthBarMesh,
                AssetProvider.Instance.registry.render.HealthBarMaterial,
                capacity: tileCapacity);
            _healthBarRenderer.Init();
            _fragmentRenderer = new FragmentRenderer(CellPivot.gameObject,
                AssetProvider.Instance.registry.render.FragmentMesh,
                AssetProvider.Instance.registry.render.FragmentMaterial,
                capacity: tileCapacity);
            _fragmentRenderer.Init();
            _shadowRectRenderer = new ObjectIndirectRenderer<ShadowID, ShadowRect>(CellPivot.gameObject,
                AssetProvider.Instance.registry.render.ShadowRectMesh,
                AssetProvider.Instance.registry.render.ShadowRectMaterial,
                capacity: tileCapacity * 5); // CellAnchor emits up to 5 shadow rects per block
            _shadowRectRenderer.SortingDepth = -0.2f;
            _shadowRectRenderer.Init();
            _dropRenderer = new DropRenderer(CellPivot.gameObject,
                AssetProvider.Instance.registry.render.ItemDropMesh,
                AssetProvider.Instance.registry.render.ItemDropMaterial,
                capacity: tileCapacity);
            _dropRenderer.Init();
            _oreRenderer = new OreRenderer(CellPivot.gameObject,
                AssetProvider.Instance.registry.render.OreMesh,
                AssetProvider.Instance.registry.render.OreMaterial,
                capacity: size*size);
            _oreRenderer.Init();
            _blockRenderer = new BlockRenderer(CellPivot.gameObject,
                AssetProvider.Instance.registry.render.BlockMesh,
                AssetProvider.Instance.registry.render.BlockMaterial,
                capacity: tileCapacity);
            _blockRenderer.SortingDepth = -1f;
            _blockRenderer.Init();
            if (behaviour != null) {
                SetBehaviour(behaviour);
            }

            if (cellArgs != null) {
                GenerateOres(cellArgs.Ores);
            }

            OnEnable();
        }
        
        private void Awake() {
            FramePivot = transform.GetChild(0);
            CellPivot = FramePivot.GetChild(0);
            _baseScale = transform.localScale;
        }
        
        private void Update() {
            _healthBarRenderer.Render();
            _fragmentRenderer.Render();
            _shadowRectRenderer.Render();
            _dropRenderer.DropPositionTime = SynchronousConveyorTime - 1/16f;
            _dropRenderer.Render();
            _oreRenderer.Render();
            _blockRenderer.Render();
            UpdateBullets();
            UpdateEntities();
            CheckCollisions();
            UpdateAliveness();
            DrawBullets(); // Cell.Bullet.cs
            DrawHitboxes(); // Cell.Debug.cs
            DrawNavigation(); // Cell.Debug.cs
            NavTree.NavTreeRebuildEvent.Invoke();
        }

        private void OnEnable() {
            if (!_initialized) return;
            _behaviour.OnEnable(this);
            MainController.Instance.InteractionManager.Register(this);
            GameEvents.OnCellPositionUpdate += OnAnyCellPositionUpdate;
            GameEvents.DamageDealt += DamagedBlockHealthBar;
            GameEvents.HealthHealed += DamagedBlockHealthBar;
        }

        private void OnDisable() {
            if (!_initialized) return;
            _behaviour.OnDisable(this);
            MainController.Instance.InteractionManager.Unregister(this);
            GameEvents.OnCellPositionUpdate -= OnAnyCellPositionUpdate;
            GameEvents.DamageDealt -= DamagedBlockHealthBar;
            GameEvents.HealthHealed -= DamagedBlockHealthBar;
        }

        private void OnDestroy() {
            _healthBarRenderer?.Release();
            _shadowRectRenderer?.Release();
            _fragmentRenderer?.Release();
            _dropRenderer?.Release();
            _oreRenderer?.Release();
            _blockRenderer?.Release();

            DOTween.Kill(this);
        }

        private void OnAnyCellPositionUpdate() {
            if (!transform || !gameObject || !gameObject.activeInHierarchy) return;
            
            transform.DOKill();
            if (this == GameStorage.Instance.CellInventory.GetTable()) {
                transform.DOMove(GameStorage.Instance.Table.transform.position, 0.25f).SetEase(Ease.InOutSine);
                transform.DOScale(1, 0.35f).SetEase(Ease.InOutQuad);
            }
            else {
                var pos = new Vector2(6.075f * ((List<Cell>)GameStorage.Instance.CellInventory.GetCells()).IndexOf(this), 0);
            
                var dif = pos - (Vector2) transform.localPosition;
                if (dif.sqrMagnitude > 0.1f) {
                    Sequence rot = DOTween.Sequence();
                    rot.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(0,0,15 * Math.Sign(dif.x)),0.15f).SetEase(Ease.OutBack));
                    rot.Append(transform.DOLocalRotateQuaternion(Quaternion.identity,0.15f).SetEase(Ease.InOutSine));
                
                }

                transform.DOLocalMove(pos, 0.25f).SetEase(Ease.InOutSine);
                transform.DOScale(0.6f, 0.35f).SetEase(Ease.InOutQuad);
            }
        }
        [Header("Tilemap")]
        public Tilemap tilemap;

        [Header("Tiles")]
        public TileBase[] tiles;

        [Header("Fill settings")]
        public int size = 8;               // n x n

        [ContextMenu("Fill Random")]
        public void Fill()
        {
            if (!tilemap || tiles == null || tiles.Length == 0)
            {
                Debug.LogWarning("Tilemap or tiles not set");
                return;
            }

            tilemap.ClearAllTiles();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var pos = new Vector3Int(
                        x,
                        y,
                        0
                    );

                    var tile = tiles[Random.Range(0, tiles.Length)];
                    tilemap.SetTile(pos, tile);
                }
            }
        }

        public void InitiateCellDestruction() {
            GameStorage.Instance.Representer.Displace();
            Destroy(gameObject);
            GameStorage.Instance.RemoveCell(this);
        }
    }
}