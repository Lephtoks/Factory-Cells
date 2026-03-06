using System;
using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Bulding;
using Cells.Object.Node;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Cell : MonoBehaviour
{
    private Dictionary<Vector2Int, CellObject> cellObjects = new();
    public ICellBehaviour behaviour;
    
    private void OnEnable() {
        GameStorage.Instance.AddCell(this);
        GameEvents.OnCellSelected += OnAnyCellSelected;
    }

    private void OnDisable() {
        GameStorage.Instance.RemoveCell(this);
        GameEvents.OnCellSelected -= OnAnyCellSelected;
    }
    

    private void OnAnyCellSelected(Cell obj) {
        Debug.Log("Cell selected eve");
        transform.DOKill();
        if (this == GameStorage.Instance.CellInventory.GetTable()) {
            transform.DOMove(GameStorage.Instance.Table.transform.position, 0.2f);
        }
        else {
            var pos = new Vector2(5 * ((List<Cell>)GameStorage.Instance.CellInventory.GetCells()).IndexOf(this), 0);
            transform.DOLocalMove(pos, 0.2f);
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
    void OnClick(Vector2 worldPos, Vector3Int tilePos, Cell cell);
}

public static class CellBehaviours
{
    public static readonly InventoryBehaviour INVENTORY = new InventoryBehaviour();
    public static readonly TableBehaviour TABLE = new TableBehaviour();
}

public class InventoryBehaviour : ICellBehaviour
{
    public void OnClick(Vector2 worldPos, Vector3Int tilePos, Cell cell) {
        GameStorage.Instance.CellInventory.PlaceOnTable(cell);
        GameEvents.InvokeCellSelection(cell);
        Debug.Log("OKKKK");
    }
}
public class TableBehaviour : ICellBehaviour
{
    public void OnClick(Vector2 worldPos, Vector3Int tilePos, Cell cell) {
        var clickedPos = new Vector2Int(tilePos.x, tilePos.y);
        Debug.Log($"Клик по тайлу в {cell.tilemap.name} на {tilePos}");
        var currentCard = GameStorage.Instance.ActiveCard;
        if (currentCard) {
            cell.TryAddObject(currentCard.CellObject.factory.Invoke(cell, clickedPos, Direction.EAST));
        }
    }
}
