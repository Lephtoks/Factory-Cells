using System.Collections.Generic;
using UnityEngine;

namespace GameRenderer
{
    public class ObjectIndirectRenderer<K, T> : IndirectRenderer<T>  where T : struct
    {
        private readonly BidirectionalDictionary<K, int> _id2Index = new BidirectionalDictionary<K, int>();

        public ObjectIndirectRenderer(GameObject gameObject, Mesh quad, Material material, int capacity = 10000) : base(gameObject, quad, material, capacity) {
        }

        public void Add(K key, T value) {
            if (_id2Index.TryGetValue(key, out int index)) {
                CPUData[index] = value;
                return;
            }
            if (Count >= Capacity)
                return;
            CPUData[Count] = value;
            _id2Index.Add(key, Count++);
        }

        public void Remove(K key) {
            if (!_id2Index.Remove(key, out int index)) return;

            Count--;
            if (index != Count) {
                CPUData[index] = CPUData[Count];
                var swapId = _id2Index.Inverse[Count];
                _id2Index[swapId] = index;
            }
        }

        public override void Release() {
            base.Release();
            _id2Index.Clear();
        }
    }
}