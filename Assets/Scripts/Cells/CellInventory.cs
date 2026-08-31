using System.Collections.Generic;
using Cells;
using UnityEngine;

public class CellInventory
{
    private List<Cell> cells = new();
    private Cell onTable;
    //
    // void Start() {
    //     SetChildren();
    // }
    //
    public void AddCell(Cell cell) {
        if (onTable == cell) { return; }
    
        cell.SetBehaviour(CellBehaviours.INVENTORY);
        cells.Add(cell);
    }
    public void RemoveCell(Cell cell) {
        cells.Remove(cell);
    }

    public IReadOnlyList<Cell> GetCells() {
        return cells;
    }

    public Cell GetTable() {
        return onTable;
    }
    public void PlaceOnTable(Cell cell) {
        if (onTable) {
            if (onTable.locked) {
                Debug.Log("Table item can't be moved");
                return;
            }
            var table = onTable;
            onTable = null;
            AddCell(table);
        }
        cells.Remove(cell);
        onTable = cell;
        onTable.SetBehaviour(CellBehaviours.TABLE);
    }
}
