using System;
using Cells;
using UI.Cards;

namespace Data
{
    public static class GameEvents
    {
        public static event Action OnCardHandUpdated;
        public static event Action OnCellPositionUpdate;
        public static event Action OnCameraUpdate;
        public static event Action<int, int> OnScreenSizeChanged;

        public static void InvokeCardHandUpdate() {
            OnCardHandUpdated?.Invoke();
        }
        public static void InvokeCellPositionUpdate() {
            OnCellPositionUpdate?.Invoke();
        }
        public static void InvokeScreenSizeChange(int width, int height) {
            OnScreenSizeChanged?.Invoke(width, height);
        }

        public static void InvokeCameraUpdate() {
            OnCameraUpdate?.Invoke();
        }
    }
}