using System;
using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Node;
using Data;
using DG.Tweening;
using Interactions;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Cells
{
    public class Cell : MonoBehaviour, ITouchable
    {
        private readonly Dictionary<Vector2Int, Block> _cellObjects = new();
        public ICellBehaviour Behaviour;
        public Transform FramePivot { get; private set; }
        public Transform CellPivot { get; private set; }

        private void Awake() {
            FramePivot = transform.GetChild(0);
            CellPivot = FramePivot.GetChild(0);
        }
        private void OnEnable() {
            GameStorage.Instance.AddCell(this);
            MainController.Instance.InteractionManager.Register(this);
            GameEvents.OnCellSelected += OnAnyCellSelected;
        }

        private void OnDisable() {
            GameStorage.Instance.RemoveCell(this);
            MainController.Instance.InteractionManager.Unregister(this);
            GameEvents.OnCellSelected -= OnAnyCellSelected;
        }

        public void OnClickBegin(CellBehaviourArguments args) {
            Behaviour.OnClickBegin(this, args);
        }
        public void OnClickRelease(CellBehaviourArguments args) {
            Behaviour.OnClickRelease(this, args);
        }
        public void OnClickMove(CellBehaviourArguments args) {
            Behaviour.OnClickMove(this, args);
        }

        private void OnAnyCellSelected(Cell obj) {
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

        public void UpdateMove() {
            var intents = new List<Intent>();
            foreach (Block cellObject in _cellObjects.Values) {
                if (cellObject is IInventory node) {
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

        public float GetDepth() {
            return DepthLayers.CELLS;
        }

        public bool CapturesClick() {
            return Behaviour == CellBehaviours.TABLE;
        }

        public bool IsSelected(Vector3 mousePos, Vector3 worldPos) {
            var cellPos = tilemap.WorldToCell(worldPos);
            return tilemap.HasTile(cellPos);
        }

        public void Select(Vector3 mousePos, Vector3 worldPos, int capturedButton, bool captured) {
            MainController.Instance.CellBehaviourArguments.CapturedButton = capturedButton;
            var cellPos = tilemap.WorldToCell(worldPos);
            
            if (GameStorage.Instance.ActiveCard && Behaviour == CellBehaviours.TABLE &&
                ( capturedButton == -1 || 
                  (!Input.GetMouseButton(MainController.Instance.CellBehaviourArguments.CapturedButton) &&
                   !Input.GetMouseButtonUp(MainController.Instance.CellBehaviourArguments.CapturedButton)))) {
                IBlockRepr instanceNodeRepr = GameStorage.Instance.NodeReprs[0];
                instanceNodeRepr.MakePhantom();
                instanceNodeRepr.SetPos(new Vector3Int(cellPos.x, cellPos.y, -1), CellPivot);
            }

            if (capturedButton != -1) {
                if (Input.GetMouseButtonDown(MainController.Instance.CellBehaviourArguments.CapturedButton)) {
                    MainController.Instance.CellBehaviourArguments.MouseBeginPos = worldPos;
                    MainController.Instance.CellBehaviourArguments.ObjectCaptured = captured;
                    MainController.Instance.CellBehaviourArguments.LocalMouseBeginPos = tilemap.WorldToLocal(worldPos);
                    OnClickBegin(MainController.Instance.CellBehaviourArguments);
                }

                if (Input.GetMouseButtonUp(MainController.Instance.CellBehaviourArguments.CapturedButton)) {
                    OnClickRelease(MainController.Instance.CellBehaviourArguments);
                }

                if (Input.GetMouseButton(MainController.Instance.CellBehaviourArguments.CapturedButton)) {
                    OnClickMove(MainController.Instance.CellBehaviourArguments);
                }
            }

            if (TryGetObject((Vector2Int)cellPos, out Block cellObject) && cellObject is IInventory inventory) {
                GameStorage.Instance.InfoCloud.transform.position = mousePos;
                GameStorage.Instance.InfoCloud.gameObject.SetActive(true);
                foreach (var itemStack in inventory.GetItems()) {
                    GameStorage.Instance.InfoCloud.TryAddIcon(itemStack);
                }
            }
            else {
                GameStorage.Instance.InfoCloud.gameObject.SetActive(false);
            }
        }
    }

    public interface ICellBehaviour
    {
        void OnClickRelease(Cell cell, CellBehaviourArguments args) {}
        void OnClickBegin(Cell cell, CellBehaviourArguments args) {}
        void OnClickMove(Cell cell, CellBehaviourArguments args) {}
    }

    public static class CellBehaviours
    {
        public static readonly InventoryBehaviour INVENTORY = new();
        public static readonly TableBehaviour TABLE = new();
    }

    public class InventoryBehaviour : ICellBehaviour
    {
        public void OnClickRelease(Cell cell, CellBehaviourArguments args) {
            GameStorage.Instance.CellInventory.PlaceOnTable(cell);
            GameEvents.InvokeCellSelection(cell);
        }
    }
    public class TableBehaviour : ICellBehaviour
    {
        public void OnClickRelease(Cell cell, CellBehaviourArguments args) {
            if (!args.ObjectCaptured) return;

            switch (args.CapturedButton) {
                case 0: {
                    var currentCard = GameStorage.Instance.ActiveCard;
                    if (!currentCard) return;

                    var reprs = GameStorage.Instance.NodeReprs;
                    while (reprs.Count > 0) {
                        var repr = reprs[^1];
                        reprs.Remove(repr);
                        if (!cell.TryAddObject(currentCard.Block.Create(cell, repr))) {
                            UnityEngine.Object.Destroy(repr.gameObject);
                        }

                    }

                    GameStorage.Instance.CreatePointerRepr();
                    break;
                }
                case 1:
                    var cellMousePoint = cell.tilemap.WorldToCell(args.WorldPos);
                    cell.RemoveObject((Vector2Int) cellMousePoint);
                    break;
            }
        }

        public void OnClickMove(Cell cell, CellBehaviourArguments args) {
            if (!args.ObjectCaptured)  return;
            
            var currentCard = GameStorage.Instance.ActiveCard;
            if (!currentCard) return;
        
            if (args.CapturedButton != 0) return;
            
            var localMousePosUnclamped = cell.tilemap.WorldToLocal(args.WorldPos);
            var localMousePos = new Vector3(
                Mathf.Clamp(localMousePosUnclamped.x, 0f, cell.size - 0.001f),
                Mathf.Clamp(localMousePosUnclamped.y, 0f, cell.size - 0.001f),
                localMousePosUnclamped.z
            );

            Vector3 localEndPoint;
            int reprs;
            var dx = 0;
            var dy = 0;
            var dir = localMousePos - args.LocalMouseBeginPos;
        
            if (Math.Abs(dir.x) > Math.Abs(dir.y)) {
                localEndPoint = new Vector2(localMousePos.x, args.LocalMouseBeginPos.y);
                reprs = Mathf.CeilToInt(Math.Max(localEndPoint.x, args.LocalMouseBeginPos.x)) - Mathf.FloorToInt(Math.Min(localEndPoint.x, args.LocalMouseBeginPos.x));
                dx = Math.Sign(dir.x);
            }
            else {
                localEndPoint = new Vector2(args.LocalMouseBeginPos.x, localMousePos.y);
                reprs = Mathf.CeilToInt(Math.Max(localEndPoint.y, args.LocalMouseBeginPos.y)) - Mathf.FloorToInt(Math.Min(localEndPoint.y, args.LocalMouseBeginPos.y));
                dy = Math.Sign(dir.y);
            }
        
            if (reprs >= 2) {
                GameStorage.Instance.RepresentationSettings.Direction = DirectionHelper.Vector2Direction(new Vector2(dx, dy));
            }
        
            GameStorage.Instance.SetAmountOfRepresentations(currentCard.Block.Representation, reprs);

            for (int i = 0; i < GameStorage.Instance.NodeReprs.Count; i++) {
                BlockRepr repr = GameStorage.Instance.NodeReprs[i];
                repr.SetPos(new Vector3Int((int)args.LocalMouseBeginPos.x + dx * i, (int)args.LocalMouseBeginPos.y + dy * i,-1), cell.CellPivot);
            }
        }
    }
}