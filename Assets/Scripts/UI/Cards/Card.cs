using System;
using System.Numerics;
using Cells.Object;
using Cells.Object.Bulding;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace UI.Cards
{
    public class Card : MonoBehaviour, 
        IPointerEnterHandler,
        IPointerExitHandler
    {
        private RectTransform _rectTransform;
        public int index;
        public CellObjectType CellObject;

        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            ApplyHandTransform(5);
            OnEnable();
            
            // TODO TEMP:
            CellObject = CellObjectTypes.CONVEYOR;
        }

        private void OnEnable() {
            GameEvents.OnCardSelected += OnAnyCardSelected;
        }

        private void OnDisable() {
            
            GameEvents.OnCardSelected -= OnAnyCardSelected;
        }
        public void Activate() {
            GameStorage.Instance.ActiveCard = this;
            GameEvents.InvokeCardSelection(this);
        }

        private void OnAnyCardSelected(Card card) {
            if (card == this) {
                ApplyHandTransform( 5, radius: 300, offset: 30f);
            }
            else {
                ApplyHandTransform( 5);
            }
        }

        public void ApplyHandTransform(
            int total,
            float radius = 300f,
            float fanAngle = 60f,
            float duration = 0.25f,
            float offset = 0f
        ) {
            if (total <= 1) {
                _rectTransform
                    .DOAnchorPos(Vector2.zero, duration)
                    .SetEase(Ease.OutCubic);

                _rectTransform
                    .DOLocalRotate(Vector3.zero, duration)
                    .SetEase(Ease.OutCubic);

                return;
            }

            float t = index / (float)(total - 1);
            float angle = Mathf.Lerp(-fanAngle / 2f, fanAngle / 2f, t);
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(rad) * (radius + offset);
            float y = Mathf.Cos(rad) * (radius + offset) - radius;

            Vector2 targetPos = new Vector2(x, y);
            Vector3 targetRot = new Vector3(0, 0, -angle);

            // Убиваем старые твины, чтобы не накапливались
            _rectTransform.DOKill();

            _rectTransform
                .DOAnchorPos(targetPos, duration)
                .SetEase(Ease.OutCubic);

            _rectTransform
                .DOLocalRotate(targetRot, duration)
                .SetEase(Ease.OutCubic);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (GameStorage.Instance.ActiveCard != this) ApplyHandTransform( 5, radius: 300, offset: 15f);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (GameStorage.Instance.ActiveCard != this) ApplyHandTransform( 5, radius: 300);
        }
    }
}