using System;
using System.Collections.Generic;
using Attributes;
using Core;
using Data;
using Data.GameManagement;
using DG.Tweening;
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

        public static Cell Create(Cell prefab, ICellBehaviour behaviour = null) {
            var cell = Instantiate(prefab);
            cell.Init(behaviour);
            return cell;
        }

        private void Init(ICellBehaviour behaviour = null) {
            _initialized = true;
            _behaviour = CellBehaviours.NONE;
            if (behaviour != null) {
                SetBehaviour(behaviour);
            }
            OnEnable();
        }
        
        private void Awake() {
            FramePivot = transform.GetChild(0);
            CellPivot = FramePivot.GetChild(0);
            _baseScale = transform.localScale;

            _droppedItemBlock = new(); // Cell.Drop.cs
        }
        
        private void Update() {
            DrawDroppedItems(); // Cell.Drop.cs
        }
        
        private void OnEnable() {
            if (!_initialized) return;
            _behaviour.OnEnable(this);
            MainController.Instance.InteractionManager.Register(this);
            GameEvents.OnCellPositionUpdate += OnAnyCellPositionUpdate;
        }

        private void OnDisable() {
            if (!_initialized) return;
            _behaviour.OnDisable(this);
            MainController.Instance.InteractionManager.Unregister(this);
            GameEvents.OnCellPositionUpdate -= OnAnyCellPositionUpdate;
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
    }
}