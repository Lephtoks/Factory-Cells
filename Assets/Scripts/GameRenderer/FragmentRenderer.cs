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
            if (Count == 0) return;

            UpdateCPU();

            base.Render();
        }

        private void UpdateCPU() {
            var fragments = CPUData.ToList();
            var nextTime = Time.time + Time.deltaTime;
            while (fragments.Count > 0)
            {
                var fragment = fragments[0];
                Debug.Log(fragments.Count);
                Debug.Log(fragment.CTime);
                Debug.Log(_time);
                if (nextTime >= Lifetime) {
                    if (_time < fragment.CTime + Lifetime && fragment.CTime + Lifetime <= nextTime) {
                        fragments.RemoveAt(0);
                        continue;
                    }
                    
                }
                if (_time < fragment.CTime && fragment.CTime <= nextTime) {
                    fragments.RemoveAt(0);
                    continue;
                }
                break;
            }
            fragments.AddRange(_toAdd);
            _toAdd.Clear();
            Count = Math.Min(fragments.Count, CPUData.Length);
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