using System;
using System.Collections;
using Attributes;
using Cells.Object;
using Core.Locals;
using Data;
using Data.GameManagement;
using Data.Offers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace UI.Cards
{
    public class Card : MonoBehaviour, IOfferable
    {
        internal RectTransform _rectTransform;
        internal Vector3 _baseScale;
        internal int _siblingIndex;
        private RectTransform _parent;
        private ICardBehaviour _behaviour;
        private bool _initialized;
        public BlockType Block;
        public UIGlow Glow;
        [SerializeField] private Image image;

        internal Action UpdateShopPosition;

        public static Card Create(Card prefab, BlockType blockType, ICardBehaviour behaviour = null) {
            var card = Instantiate(prefab);
            card.Init(blockType, behaviour);
            return card;
        }

        private void Init(BlockType blockType, ICardBehaviour behaviour = null) {
            _initialized = true;
            _behaviour = CardBehaviours.NONE;
            if (behaviour != null) {
                SetBehaviour(behaviour);
            }
            Block = blockType;
            OnEnable();
        }
        
        // Mono
        
        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _baseScale = _rectTransform.localScale;
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private void Start() {
            image.sprite = Block.Def.TextureForUI;
        }

        internal void SetParent(Transform parent) {
            _rectTransform.SetParent(parent);
            _parent = (RectTransform) parent;
        }

        private void OnEnable() {
            if (_initialized) _behaviour.OnEnable(this);
        }

        private void OnDisable() {
            if (_initialized) _behaviour.OnDisable(this);
        }

        // Behaviour
        

        public void SetBehaviour(ICardBehaviour behaviour) {
            _behaviour.OnDisable(this);
            _behaviour = behaviour;
            _behaviour.OnEnable(this);
            _behaviour.InitBehaviour(this);
        }

        public ICardBehaviour GetBehaviour() {
            return _behaviour;
        }
        
        public void OnClick() {
            _behaviour.OnClick(this);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _behaviour.OnPointerEnter(eventData, this);
        }

        public void OnPointerExit(PointerEventData eventData) {
            _behaviour.OnPointerExit(eventData, this);
        }
        
        // Hand logic

        public void ApplyHandTransform(
            float radius = 300f,
            float fanAngle = 90f,
            float duration = 0.25f,
            float offset = 0f,
            float lineLength = 400f
        ) {
            var cards = GameStorage.Instance.GetCards();
            var index = cards.IndexOf(this);
            var total = cards.Count;
            
            if (total <= 1) {
                _rectTransform.DOKill();
                
                _rectTransform
                    .DOAnchorPos3D(Vector3.zero, duration)
                    .SetEase(Ease.OutCubic);

                _rectTransform
                    .DOLocalRotate(Vector3.zero, duration)
                    .SetEase(Ease.OutCubic);
            
                Vector3 localScale = transform.localScale;
                _rectTransform
                    .DOScale(new Vector3(localScale.x / localScale.y, 1, 1)*3.5f, duration)
                    .SetEase(Ease.OutCubic);

                return;
            }

            float t = index / (float)(total - 1);
            fanAngle = Mathf.Min(fanAngle, 15*total);
            float angle = Mathf.Lerp(-fanAngle / 2f, fanAngle / 2f, t);
            float rad = angle * Mathf.Deg2Rad;
            
            lineLength = Mathf.Min(lineLength, 50*total);
            float lineX = Mathf.Lerp(-lineLength / 2f, lineLength / 2f, t);

            float x = lineX + Mathf.Sin(rad) * (radius + offset);
            float y = Mathf.Cos(rad) * (radius + offset) - radius;

            Vector2 targetPos = new Vector2(x, y);
            Vector2 dir = targetPos - new Vector2(lineX, -radius);
            
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

            Vector3 targetRot = new Vector3(0, 0, rot);

            _rectTransform.DOKill();

            _rectTransform
                .DOAnchorPos3D(new Vector3(targetPos.x, targetPos.y, 0), duration)
                .SetEase(Ease.OutCubic);

            _rectTransform
                .DOLocalRotate(targetRot, duration)
                .SetEase(Ease.OutCubic);


            {
                Vector3 localScale = transform.localScale;
                _rectTransform
                    .DOScale(new Vector3(localScale.x / localScale.y, 1, 1)*3.5f, duration)
                    .SetEase(Ease.OutCubic);
            }
        }
        
        // Shop logic

        public void AddToOffer(int row, int col, int totalRows, int totalCols) {
            UpdateShopPosition = () => ShopUpdater(row, col, totalRows, totalCols);
            Glow.Show();
            SetBehaviour(CardBehaviours.SHOP);
            OnShopAppearance(row, col, totalRows, totalCols);
        }
        public void DestroyInOffer() {
            transform.DOKill();
            Glow.Hide();
            Destroy(gameObject);
        }

        public void SelectedInOffer() {
            Glow.Hide();
            SetBehaviour(CardBehaviours.HAND);
            GameStorage.Instance.AddCard(this);
        }

        internal void OnScreenSizeChanged(int width, int height)
        {
            UpdateShopPosition();
        }
        public void ShopUpdater(int row, int col, int totalRows, int totalCols)
        {
            float parentWidth = _parent.rect.width;
            float parentHeight = _parent.rect.height;

            float cardWidth = _rectTransform.rect.width;
            float cardHeight = _rectTransform.rect.height;

            
            float minX = -parentWidth * 0.3f;
            float maxX = parentWidth * 0.3f;

            float minY = -parentHeight * 0.3f;
            float maxY = parentHeight * 0.3f;

            float availableWidth = maxX - minX;
            float availableHeight = maxY - minY;

            
            
            float scaleByHeight = availableHeight / cardHeight;
            
            
            
            float scaleByWidth;

            if (totalCols <= 1)
            {
                scaleByWidth = availableWidth / cardWidth;
            }
            else
            {
                float step = availableWidth / (totalCols - 1);
                scaleByWidth = step / cardWidth;
            }

            float scale = Mathf.Min(scaleByWidth, scaleByHeight) * 0.9f;
            
            
            float x;

            if (totalCols <= 1)
            {
                x = 0f;
            }
            else
            {
                float step = availableWidth / (totalCols - 1);
                x = minX + step * col;
            }

            float y = 0f;
            
            _rectTransform.DOKill();
            _rectTransform
                .DOScale(_baseScale * scale, 0.3f)
                .SetEase(Ease.OutCubic);

            _rectTransform
                .DOAnchorPos3D(new Vector3(x, y, 0), 0.3f)
                .SetEase(Ease.OutCubic);
        }
        public void OnShopAppearance(int row, int col, int totalRows, int totalCols)
        {
            float parentWidth = _parent.rect.width;
            float parentHeight = _parent.rect.height;

            float cardWidth = _rectTransform.rect.width;
            float cardHeight = _rectTransform.rect.height;

            
            float minX = -parentWidth * 0.3f;
            float maxX = parentWidth * 0.3f;

            float minY = -parentHeight * 0.3f;
            float maxY = parentHeight * 0.3f;

            float availableWidth = maxX - minX;
            float availableHeight = maxY - minY;

            
            
            float scaleByHeight = availableHeight / cardHeight;
            
            
            
            float scaleByWidth;

            if (totalCols <= 1)
            {
                scaleByWidth = availableWidth / cardWidth;
            }
            else
            {
                float step = availableWidth / (totalCols - 1);
                scaleByWidth = step / cardWidth;
            }

            float scale = Mathf.Min(scaleByWidth, scaleByHeight) * 0.9f;
            
            
            float x;

            if (totalCols <= 1)
            {
                x = 0f;
            }
            else
            {
                float step = availableWidth / (totalCols - 1);
                x = minX + step * col;
            }

            float y = 0f;
                
            _rectTransform.localScale = _baseScale * scale * 1.5f;
            _rectTransform.anchoredPosition3D = new Vector3(x, y, 0);
        }
        

        public float GetShopScaleFactor() {
            var total = GameStorage.Instance.GetOffer().Count;
            
            float parentWidth = _parent.rect.width;
            float parentHeight = _parent.rect.height;

            float cardWidth = _rectTransform.rect.width;
            float cardHeight = _rectTransform.rect.height;

            
            float minX = -parentWidth * 0.3f;
            float maxX = parentWidth * 0.3f;

            float minY = -parentHeight * 0.3f;
            float maxY = parentHeight * 0.3f;

            float availableWidth = maxX - minX;
            float availableHeight = maxY - minY;

            
            float scaleByHeight = availableHeight / cardHeight;
            
            
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
        
        // Events

        public void OnAnyCardSelected(Card card) {
        }
        
        public void OnHandUpdate() {
            if (this == GameStorage.Instance.ActiveCard) {
                ApplyHandTransform(radius: 300, offset: 30f);
            }
            else {
                ApplyHandTransform();
            }
        }
    }

    public interface ICardBehaviour
    {
        public void OnClick(Card card) {}
        public void OnEnable(Card card) {}
        public void OnDisable(Card card) {}
        public void InitBehaviour(Card card) {}
        public void OnPointerEnter(PointerEventData eventData, Card card) {}
        public void OnPointerExit(PointerEventData eventData, Card card) {}
    }

    public static class CardBehaviours
    {
        public static readonly EmptyCardBehaviour NONE = new EmptyCardBehaviour();
        public static readonly HandCardBehaviour HAND = new HandCardBehaviour();
        public static readonly ShopCardBehaviour SHOP = new ShopCardBehaviour();
    }

    public class EmptyCardBehaviour : ICardBehaviour { }

    public class HandCardBehaviour : ICardBehaviour
    {
        public void OnClick(Card card) {
            GameStorage.Instance.SetActiveCard(card);
            GameEvents.InvokeCardHandUpdate();
        }

        public void InitBehaviour(Card card) {
            card.SetParent(GameLocalBootstrap.Instance.canvasCardHolder.transform);
            if (GameStorage.Instance.ActiveCard != card) card.ApplyHandTransform(radius: 300);
        }

        public void OnEnable(Card card) {
            GameEvents.OnCardHandUpdated += card.OnHandUpdate;
        }

        public void OnDisable(Card card) {
            GameEvents.OnCardHandUpdated -= card.OnHandUpdate;
        }
        
        public void OnPointerEnter(PointerEventData eventData, Card card) {
            if (GameStorage.Instance.ActiveCard != card) card.ApplyHandTransform(radius: 300, offset: 15f);
            card._siblingIndex = card.transform.GetSiblingIndex();
            card.transform.SetAsLastSibling();
        }

        public void OnPointerExit(PointerEventData eventData, Card card) {
            if (GameStorage.Instance.ActiveCard != card) card.ApplyHandTransform(radius: 300);
            card.transform.SetSiblingIndex(card._siblingIndex);
        }
    }

    public class ShopCardBehaviour : ICardBehaviour
    {
        public void OnClick(Card card) {
            GameStorage.Instance.GetOffer().SelectAndClose(card);
        }

        public void InitBehaviour(Card card) {
            card.SetParent(GameLocalBootstrap.Instance.shopScreen.transform);
            card.UpdateShopPosition();
        }

        public void OnEnable(Card card) {
            GameEvents.OnScreenSizeChanged += card.OnScreenSizeChanged;
        }

        public void OnDisable(Card card) {
            GameEvents.OnScreenSizeChanged -= card.OnScreenSizeChanged;
        }

        public void OnPointerEnter(PointerEventData eventData, Card card) {
            card._rectTransform
                .DOScale(card._baseScale * card.GetShopScaleFactor() + new Vector3(0.25f, 0.25f, 0), 0.2f)
                .SetEase(Ease.OutCubic);
        }

        public void OnPointerExit(PointerEventData eventData, Card card) {
            card.UpdateShopPosition();
        }
    }

}