using System;
using Cells;
using Cells.Object;
using UI.Cards;

namespace Data
{
    public static class GameEvents
    {
        public static event Action OnCardHandUpdated;
        public static event Action OnCellPositionUpdate;
        public static event Action OnCameraUpdate;
        public static event Action<int, int> OnScreenSizeChanged;
        public static event Action<IHealth, float> DamageDealt;
        public static event Action<IHealth, float> HealthHealed;

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
        public static void InvokeDamageDealt(IHealth health, float value) {
            DamageDealt?.Invoke(health, value);
        }
        public static void InvokeHealthHealed(IHealth health, float value) {
            HealthHealed?.Invoke(health, value);
        }
    }
}