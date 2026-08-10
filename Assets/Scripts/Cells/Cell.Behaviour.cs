using System;
using System.Collections.Generic;
using Cells.Object;
using Core.Locals;
using Data;
using Data.GameManagement;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private ICellBehaviour _behaviour;
        
        public void SetBehaviour(ICellBehaviour behaviour) {
            _behaviour.OnDisable(this);
            _behaviour = behaviour;
            _behaviour.OnEnable(this);
            _behaviour.InitBehaviour(this);
        }

        public void OnClickBegin(CellBehaviourArguments args) {
            _behaviour.OnClickBegin(this, args);
        }
        public void OnClickRelease(CellBehaviourArguments args) {
            _behaviour.OnClickRelease(this, args);
        }
        public void OnClickMove(CellBehaviourArguments args) {
            _behaviour.OnClickMove(this, args);
        }
    }

    public interface ICellBehaviour
    {
        void OnClickRelease(Cell cell, CellBehaviourArguments args) {}
        void OnClickBegin(Cell cell, CellBehaviourArguments args) {}
        void OnClickMove(Cell cell, CellBehaviourArguments args) {}
        void InitBehaviour(Cell cell) {}
        void OnEnable(Cell cell) {}
        void OnDisable(Cell cell) {}
    }

    public static class CellBehaviours
    {
        public static readonly EmptyCellBehaviour NONE = new EmptyCellBehaviour();
        public static readonly InventoryBehaviour INVENTORY = new();
        public static readonly TableBehaviour TABLE = new();
        public static readonly ShopCellBehaviour SHOP = new();
    }

    public class EmptyCellBehaviour : ICellBehaviour { }

    public class ShopCellBehaviour : ICellBehaviour
    {
        public void OnEnable(Cell cell) {
            GameEvents.OnScreenSizeChanged += cell.OnScreenSizeChanged;
            GameEvents.OnCameraUpdate += cell.OnCameraUpdate;
        }

        public void OnDisable(Cell cell) {
            GameStorage.Instance.MoveOnDefaultLayer(cell.gameObject);
            GameEvents.OnScreenSizeChanged -= cell.OnScreenSizeChanged;
            GameEvents.OnCameraUpdate -= cell.OnCameraUpdate;
        }

        public void InitBehaviour(Cell cell) {
            cell.UpdateShopPosition();
            GameStorage.Instance.MoveOnOfferLayer(cell.gameObject);
        }

        public void OnClickRelease(Cell cell, CellBehaviourArguments args) {
            switch (args.CapturedButton) {
                case 0:
                    GameStorage.Instance.GetOffer().SelectAndClose(cell);
                    break;
            }
        }
    }

    public class InventoryBehaviour : ICellBehaviour
    {
        public void InitBehaviour(Cell cell) {
            cell.transform.SetParent(GameLocalBootstrap.Instance.CellHolder.transform);
        }
        
        public void OnClickRelease(Cell cell, CellBehaviourArguments args) {
            GameStorage.Instance.CellInventory.PlaceOnTable(cell);
            GameEvents.InvokeCellPositionUpdate();
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
                    var added = new List<Block>();
                    while (reprs.Count > 0) {
                        var repr = reprs[^1];
                        reprs.Remove(repr);
                        Block block = currentCard.Block.Create(cell, repr);
                        if (!cell.TryAddObject(block)) {
                            UnityEngine.Object.Destroy(repr.gameObject);
                        }
                        else {
                            cell.BlockUpdate(block);
                        }

                    }

                    foreach (var block in added) {
                        cell.BlockUpdate(block);
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