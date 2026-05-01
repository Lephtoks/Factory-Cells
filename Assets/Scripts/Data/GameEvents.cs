using System;
using UI.Cards;

namespace Data
{
    public static class GameEvents
    {
        public static event Action<Card> OnCardSelected;
        public static event Action OnCardHandUpdated;
        public static event Action<Cell> OnCellSelected;

        public static void InvokeCardSelection(Card obj) {
            OnCardSelected?.Invoke(obj);
        }
        public static void InvokeCardHandUpdate() {
            OnCardHandUpdated?.Invoke();
        }
        public static void InvokeCellSelection(Cell obj) {
            OnCellSelected?.Invoke(obj);
        }
    }
}