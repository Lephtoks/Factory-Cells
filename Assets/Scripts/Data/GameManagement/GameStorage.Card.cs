using System.Collections.Generic;
using Cells.Object;
using UI.Cards;
using Object = UnityEngine.Object;

namespace Data.GameManagement
{
    public partial class GameStorage
    {
        public Card ActiveCard {private set; get;}
        public BlockBuildOption BuildOption = new();
        private readonly List<Card> _cardsInHand = new();
        
        public void AddCard(Card card) {
            _cardsInHand.Add(card);
            GameEvents.InvokeCardHandUpdate();
        }

        public void RemoveCard(BlockType type) {
            foreach (var cell in _cardsInHand) {
                if (cell.Block != type) continue;
                
                _cardsInHand.Remove(cell);
                Object.Destroy(cell.gameObject);
                return;
            }
            GameEvents.InvokeCardHandUpdate();
        }

        public List<Card> GetCards() {
            return _cardsInHand;
        }

        public void SetActiveCard(Card card) {
            ActiveCard = card;
            BuildOption.ActiveCardUpdated();
        }
    }
}