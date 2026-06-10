using System;
using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Node;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Cell : MonoBehaviour
{
    private Dictionary<Vector2Int, CellObject> cellObjects = new();
    public ICellBehaviour behaviour;
    public Transform FramePivot { get; private set; }
    public Transform CellPivot { get; private set; }

    private void Awake() {
        FramePivot = transform.GetChild(0);
        CellPivot = FramePivot.GetChild(0);
    }
    private void OnEnable() {
        GameStorage.Instance.AddCell(this);
        GameEvents.OnCellSelected += OnAnyCellSelected;
    }

    private void OnDisable() {
        GameStorage.Instance.RemoveCell(this);
        GameEvents.OnCellSelected -= OnAnyCellSelected;
    }

    public void OnClickBegin(CellBehaviourArguments args) {
        behaviour.OnClickBegin(this, args);
    }
    public void OnClickRelease(CellBehaviourArguments args) {
        behaviour.OnClickRelease(this, args);
    }
    public void OnClickMove(CellBehaviourArguments args) {
        behaviour.OnClickMove(this, args);
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

    public bool TryAddObject(CellObject cellObject) {
        if (!IsTileEmpty(cellObject.Position)) return false;
        cellObjects.Add(cellObject.Position, cellObject);

        cellObject.WhenBeingAddedToCell();
        return true;
    }
    public bool TryGetObject(Vector2Int position, out CellObject cellObject) {
        return cellObjects.TryGetValue(position, out cellObject);
    }

    public bool IsTileEmpty(Vector2Int position) {
        return !cellObjects.ContainsKey(position);
    }

    public void UpdateMove() {
        var intents = new List<Intent>();
        foreach (CellObject cellObject in cellObjects.Values) {
            if (cellObject is ICellNode node) {
                node.ResetIntent();
                node.GenerateIntent();
                if (node.GetIntent() != null) intents.Add(node.GetIntent());
            }
        }

        foreach (Intent intent in intents) {
            intent.Do();
        }
        foreach (CellObject cellObject in cellObjects.Values) {
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
        var currentCard = GameStorage.Instance.ActiveCard;
        if (currentCard) {
            var reprs = GameStorage.Instance.NodeReprs;
            while (reprs.Count > 0) {
                var repr = reprs[^1];
                reprs.Remove(repr);
                if (!cell.TryAddObject(currentCard.CellObject.Factory.Invoke(cell, repr))) {
                    Object.Destroy(repr.gameObject);
                }
                
            }
            GameStorage.Instance.CreatePointerRepr();
        }
    }

    public void OnClickMove(Cell cell, CellBehaviourArguments args) {
        
        
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
        GameStorage.Instance.SetAmountOfRepresentations(GameStorage.Instance.ActiveCard.CellObject.Representation, reprs);

        for (int i = 0; i < GameStorage.Instance.NodeReprs.Count; i++) {
            CellNodeRepr repr = GameStorage.Instance.NodeReprs[i];
            repr.SetPos(new Vector3Int((int)args.LocalMouseBeginPos.x + dx * i, (int)args.LocalMouseBeginPos.y + dy * i,-1), cell.CellPivot);
        }


        // int x = Mathf.FloorToInt(startCellPos.x);
        // int y = Mathf.FloorToInt(startCellPos.y);
        //
        // int endX = Mathf.FloorToInt(worldCellPos.x);
        // int endY = Mathf.FloorToInt(worldCellPos.y);
        //
        // int stepX = dir.x > 0 ? 1 : -1;
        // int stepY = dir.y > 0 ? 1 : -1;
        //
        // float tDeltaX = dir.x == 0 ? float.PositiveInfinity : Mathf.Abs(1f / dir.x);
        // float tDeltaY = dir.y == 0 ? float.PositiveInfinity : Mathf.Abs(1f / dir.y);
        //
        // float nextVertical = dir.x > 0
        //     ? (x + 1 - startCellPos.x)
        //     : (startCellPos.x - x);
        //
        // float nextHorizontal = dir.y > 0
        //     ? (y + 1 - startCellPos.y)
        //     : (startCellPos.y - y);
        //
        // float tMaxX = dir.x == 0 ? float.PositiveInfinity : tDeltaX * nextVertical;
        // float tMaxY = dir.y == 0 ? float.PositiveInfinity : tDeltaY * nextHorizontal;
        //
        // var tileDelta = tMaxX < tMaxY ? new Vector2Int(stepX, 0) : new Vector2Int(0, stepY);
        //
        // ProcessTile(startCellPos, dir, cell, (Vector2Int) tilePos, tileDelta);
        //
        // while (x != endX || y != endY) {
        //     if (tMaxX < tMaxY)
        //     {
        //         tMaxX += tDeltaX;
        //         x += stepX;
        //     }
        //     else
        //     {
        //         tMaxY += tDeltaY;
        //         y += stepY;
        //     }
        //     
        //     tileDelta = tMaxX < tMaxY ? new Vector2Int(stepX, 0) : new Vector2Int(0, stepY);
        //     var tilePosInLocalWorld = new Vector3(x, y);
        //     ProcessTile(tilePosInLocalWorld, dir, cell, (Vector2Int) tilePos, tileDelta);
        // }
    }
    // private static void ProcessTile(Vector3 tilePosInLocalWorld, Vector2 delta, Cell cell, Vector2Int currentTilePos, Vector2Int tileDelta)
    // {        
    //     var localToCell = (Vector2Int)cell.tilemap.LocalToCell(tilePosInLocalWorld);
    //     if (localToCell == currentTilePos) return;
    //     var currentCard = GameStorage.Instance.ActiveCard;
    //     if (currentCard) {
    //         cell.TryAddObject(currentCard.CellObject.Factory.Invoke(cell, localToCell, DirectionHelper.Vector2Direction(tileDelta)));
    //     }
    // }
}
