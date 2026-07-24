using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Attributes
{
    public class UIGlow : MonoBehaviour
    {
        private Vector3 _initialScale;
        [CanBeNull] private RectTransform _rectTransform;
        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _initialScale = _rectTransform ? _rectTransform.localScale : transform.localScale;
            gameObject.SetActive(false);
            Debug.Log(_initialScale);
        }

        public void Show() {
            gameObject.SetActive(true);
            if (_rectTransform) {
                _rectTransform.localScale = _initialScale * 0.25f;
                _rectTransform.DOScale(_initialScale, 1).SetEase(Ease.OutQuad);
            }
            else {
                transform.localScale = _initialScale * 0.25f;
                transform.DOScale(_initialScale, 1).SetEase(Ease.OutQuad);
            }
        }

        public void Hide() {
            if (_rectTransform) {
                _rectTransform.DOKill();
            }
            else {
                transform.DOKill();
            }
            gameObject.SetActive(false);
        }
    }
}
