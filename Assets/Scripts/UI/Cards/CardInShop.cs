using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Cards
{
    public class CardInShop : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        private RectTransform _rectTransform;
        private RectTransform _parent;

        public int index;
        public int total;

        private Vector3 _baseScale;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parent = (RectTransform)_rectTransform.parent;

            _baseScale = _rectTransform.localScale;

            // фиксируем центр как систему координат
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);

            UpdatePosition();
        }

        private void OnEnable()
        {
            GameEvents.OnScreenSizeChanged += OnScreenSizeChanged;
        }

        private void OnDisable()
        {
            GameEvents.OnScreenSizeChanged -= OnScreenSizeChanged;
        }

        private void OnScreenSizeChanged(int width, int height)
        {
            UpdatePosition();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _rectTransform
                .DOScale(_baseScale * GetScaleFactor() + new Vector3(0.25f, 0.25f, 0), 0.2f)
                .SetEase(Ease.OutCubic);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UpdatePosition();
        }

        public float GetScaleFactor() {
            float parentWidth = _parent.rect.width;
            float parentHeight = _parent.rect.height;

            float cardWidth = _rectTransform.rect.width;
            float cardHeight = _rectTransform.rect.height;

            // -----------------------
            // ГРАНИЦЫ (0.2 .. 0.8 по X, 0.1 .. 0.9 по Y)
            // -----------------------
            float minX = -parentWidth * 0.3f;
            float maxX = parentWidth * 0.3f;

            float minY = -parentHeight * 0.3f;
            float maxY = parentHeight * 0.3f;

            float availableWidth = maxX - minX;
            float availableHeight = maxY - minY;

            // -----------------------
            // SCALE ПО ВЫСОТЕ
            // -----------------------
            float scaleByHeight = availableHeight / cardHeight;

            // -----------------------
            // SCALE ПО ШИРИНЕ
            // -----------------------
            float scaleByWidth;

            if (total <= 1)
            {
                scaleByWidth = availableWidth / cardWidth;
            }
            else
            {
                float step = availableWidth / (total - 1);
                scaleByWidth = step / cardWidth;
            }

            return Mathf.Min(scaleByWidth, scaleByHeight) * 0.9f;
        }
        public void UpdatePosition()
        {
            float parentWidth = _parent.rect.width;
            float parentHeight = _parent.rect.height;

            float cardWidth = _rectTransform.rect.width;
            float cardHeight = _rectTransform.rect.height;

            // -----------------------
            // ГРАНИЦЫ (0.2 .. 0.8 по X, 0.1 .. 0.9 по Y)
            // -----------------------
            float minX = -parentWidth * 0.3f;
            float maxX = parentWidth * 0.3f;

            float minY = -parentHeight * 0.3f;
            float maxY = parentHeight * 0.3f;

            float availableWidth = maxX - minX;
            float availableHeight = maxY - minY;

            // -----------------------
            // SCALE ПО ВЫСОТЕ
            // -----------------------
            float scaleByHeight = availableHeight / cardHeight;

            // -----------------------
            // SCALE ПО ШИРИНЕ
            // -----------------------
            float scaleByWidth;

            if (total <= 1)
            {
                scaleByWidth = availableWidth / cardWidth;
            }
            else
            {
                float step = availableWidth / (total - 1);
                scaleByWidth = step / cardWidth;
            }

            float scale = Mathf.Min(scaleByWidth, scaleByHeight) * 0.9f;
            
            // -----------------------
            // ПОЗИЦИЯ
            // -----------------------
            float x;

            if (total <= 1)
            {
                x = 0f;
            }
            else
            {
                float step = availableWidth / (total - 1);
                x = minX + step * index;
            }

            float y = 0f;

            // -----------------------
            // АНИМАЦИЯ
            // -----------------------
            _rectTransform
                .DOScale(_baseScale * scale, 0.3f)
                .SetEase(Ease.OutCubic);

            _rectTransform
                .DOAnchorPos(new Vector2(x, y), 0.3f)
                .SetEase(Ease.OutCubic);
        }
    }
}