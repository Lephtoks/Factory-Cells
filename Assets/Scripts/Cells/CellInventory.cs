using System.Collections.Generic;
using Cells;

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
    
        cell.Behaviour = CellBehaviours.INVENTORY;
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
            var table = onTable;
            onTable = null;
            AddCell(table);
        }
        cells.Remove(cell);
        onTable = cell;
        onTable.Behaviour = CellBehaviours.TABLE;
    }
}
