using System;
using System.Collections.Generic;
using Economics;

namespace Data
{
    public class CurrencyData
    {
        private readonly Dictionary<Currency, int> _items = new();
        private int _wind;

        public event Action<Currency, int> Changed;

        public IReadOnlyDictionary<Currency, int> Items => _items;

        public int Wind {
            get => _wind;
            set => _wind = value;
        }

        public int Get(Currency currency) {
            return _items.TryGetValue(currency, out var count) ? count : 0;
        }

        public void Set(Currency currency, int count) {
            if (count < 0) count = 0;

            if (count == 0) {
                if (!_items.Remove(currency)) return;
                Changed?.Invoke(currency, 0);
                return;
            }

            if (_items.TryGetValue(currency, out var current) && current == count) return;

            _items[currency] = count;
            Changed?.Invoke(currency, count);
        }

        public void Add(Currency currency, int count) {
            if (count == 0) return;
            Set(currency, Get(currency) + count);
        }

        public void Remove(Currency currency, int count) {
            if (count <= 0) return;
            Set(currency, Get(currency) - count);
        }
    }
}
