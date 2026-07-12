using System.Collections.Generic;
using Cells;

namespace Data.GameManagement
{
    public partial class GameStorage
    {
        private readonly List<Cell> _tilemaps = new();
        public readonly CellInventory CellInventory = new();

        public void AddCell(Cell cell) {
            _tilemaps.Add(cell);
            CellInventory.AddCell(cell);
        }

        public void RemoveCell(Cell cell) {
            _tilemaps.Remove(cell);
            CellInventory.RemoveCell(cell);
        }

        public IReadOnlyList<Cell> GetCells() {
            return _tilemaps;
        }
    }
}