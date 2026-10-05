using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace GameRenderer
{
    public class FragmentRenderer : IndirectRenderer<Fragment>
    {
        public FragmentRenderer(GameObject gameObject, Mesh quad, Material material, int capacity = 10000) : base(
            gameObject, quad, material, capacity) {
        }

        private int _pointer;
        private const float Lifetime = 4;
        private float _time;
        private List<Fragment> _toAdd = new List<Fragment>();
        public override void Render() {
            UpdateCPU();
            base.Render();
        }private void UpdateCPU()
        {
            var nextTime = _time + Time.deltaTime;
            // nextTime may be ≥ Lifetime; we normalise only after the age tests.

            var fragments = new List<Fragment>(Count);
            for (int i = 0; i < Count - _toAdd.Count; i++)
                fragments.Add(CPUData[i]);

            int removeCount = 0;
            for (int i = 0; i < fragments.Count; i++)
            {
                if (nextTime >= Lifetime) {
                    if (_time < fragments[i].CTime + Lifetime && fragments[i].CTime + Lifetime <= nextTime) {
                        removeCount++;
                        continue;
                    }
                }
                if (_time < fragments[i].CTime && fragments[i].CTime <= nextTime)
                    removeCount++;
                else
                    break;   // list is sorted by CTime
            }

            if (removeCount > 0)
                fragments.RemoveRange(0, removeCount);

            fragments.AddRange(_toAdd);
            _toAdd.Clear();

            Count = Math.Min(fragments.Count, CPUData.Length);
            if (Count > 0)
                fragments.CopyTo(0, CPUData, 0, Count);

            _time = nextTime % Lifetime;
            Properties.SetFloat("_GlobalTime", _time);
        }


        public override void Init() {
            base.Init();
            Properties.SetTexture("_DestroyedBlockSprite", AssetProvider.Instance.registry.render.FragmentTexture);
        }

        public void Add(Vector2 position) {
            Count += 1;
            _toAdd.Add(new Fragment { CTime = _time,  Position = position });
        }
    }

    public struct Fragment
    {
        public Vector2 Position;
        public float CTime;
    }
}