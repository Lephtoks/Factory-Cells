using Cells.Object.Building;

namespace Cells
{
    public struct CellAnchorLocker : ICellLocker
    {
        public bool IsLocked => true;
    }
}