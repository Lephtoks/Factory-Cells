using Data.GameManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Cards
{
    public class CardHolder : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler
    {
        private Card _hoveredCard;

        public bool IsCardHovered() {
            return _hoveredCard != null;
        }
        public void OnPointerExit(PointerEventData eventData) {
            if (_hoveredCard != null) {
                _hoveredCard.OnPointerExit(eventData);
            }
            _hoveredCard = null;
        }

        public void OnPointerMove(PointerEventData eventData) {
            var top = GetTopCard(eventData.position);
                
            if (top == _hoveredCard) return;
        
            if (_hoveredCard != null)
                _hoveredCard.OnPointerExit(eventData);
        
            _hoveredCard = top;
        
            if (_hoveredCard != null)
                _hoveredCard.OnPointerEnter(eventData);
            
        }
        private Card GetTopCard(Vector2 screenPos) {
            Card top = null;
            int topIndex = 0;

            var list = GameStorage.Instance.GetCards();
            for (int i = 0; i < list.Count; i++) {
                Card card = list[i];
                
                if (RectTransformUtility.RectangleContainsScreenPoint(card._rectTransform, screenPos)) {
                    if (top == null || i > topIndex) {
                        top = card;
                        topIndex = i;
                    }
                }
            }

            return top;
        }
    }
}