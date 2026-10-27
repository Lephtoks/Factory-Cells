using System.Collections.Generic;
using System.Linq;
using Data.GameManagement;
using Economics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform top;
        [SerializeField] private RectTransform bottom;
        [SerializeField] private CurrencyRowView rowPrefab;

        private readonly Dictionary<Currency, CurrencyRowView> _rows = new();
        private RectTransform _rectTransform;
        private VerticalLayoutGroup _layout;
        private float _rowHeight;
        private float _topHeight;
        private float _bottomHeight;

        private void Awake() {
            _rectTransform = (RectTransform) transform;
            _layout = content.GetComponent<VerticalLayoutGroup>();
        }

        private void Start() {
            if (GameStorage.Instance == null) return;

            _rowHeight = ((RectTransform) rowPrefab.transform).rect.height;
            _topHeight = top.rect.height;
            _bottomHeight = bottom.rect.height;

            Rebuild();
            GameStorage.Instance.CurrencyData.Changed += OnCurrencyChanged;
        }

        private void OnDestroy() {
            if (GameStorage.Instance != null) {
                GameStorage.Instance.CurrencyData.Changed -= OnCurrencyChanged;
            }
        }

        private void OnCurrencyChanged(Currency currency, int count) {
            if (count <= 0) {
                if (_rows.Remove(currency, out var removed)) {
                    Destroy(removed.gameObject);
                }
            }
            else {
                if (!_rows.TryGetValue(currency, out var row)) {
                    row = Instantiate(rowPrefab, content);
                    _rows[currency] = row;
                }
                row.Set(currency, count);
                ReorderRows();
            }

            UpdateSize();
        }

        private void Rebuild() {
            foreach (var row in _rows.Values) {
                Destroy(row.gameObject);
            }
            _rows.Clear();

            foreach (var pair in GameStorage.Instance.CurrencyData.Items.OrderBy(pair => pair.Key)) {
                if (pair.Value <= 0) continue;

                var row = Instantiate(rowPrefab, content);
                row.Set(pair.Key, pair.Value);
                _rows.Add(pair.Key, row);
            }

            ReorderRows();
            UpdateSize();
        }

        private void ReorderRows() {
            var index = 0;
            foreach (var key in _rows.Keys.OrderBy(key => key)) {
                _rows[key].transform.SetSiblingIndex(index++);
            }
        }

        private void UpdateSize() {
            var height = _topHeight + _bottomHeight + _layout.padding.vertical;
            if (_rows.Count > 0) {
                height += _rows.Count * _rowHeight + (_rows.Count - 1) * _layout.spacing;
            }
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, height);
        }
    }
}
