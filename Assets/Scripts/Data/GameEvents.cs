using System;
using UI.Cards;

namespace Data
{
    public static class GameEvents
    {
        public static event Action OnCardHandUpdated;
        public static event Action<Cell> OnCellSelected;
        public static event Action<int, int> OnScreenSizeChanged;

        public static void InvokeCardHandUpdate() {
            OnCardHandUpdated?.Invoke();
        }
        public static void InvokeCellSelection(Cell obj) {
            OnCellSelected?.Invoke(obj);
        }
        public static void InvokeScreenSizeChange(int width, int height) {
            OnScreenSizeChanged.Invoke(width, height);
        }
    }
}