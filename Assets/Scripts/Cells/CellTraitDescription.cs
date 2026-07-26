using System;
using System.Collections.Generic;
using Core;
using ScriptableObjects;
using UnityEngine;

namespace Cells
{
    public class CellTraitDescription : MonoBehaviour
    {
        private readonly List<SpriteRenderer> _traitObjects = new();
        private readonly Dictionary<Type, SpriteRenderer> _dynamicTraits = new();
        private readonly Dictionary<CellStaticTraits, SpriteRenderer> _staticTraits = new();

        public void UpdatePositions() {
            if (_traitObjects.Count == 0) return;
            var scaleX = _traitObjects[0].bounds.size.x;
            float gap = 0.2f * scaleX;
            float totalWidth = _traitObjects.Count * scaleX + (_traitObjects.Count - 1) * gap;
            float offset = -totalWidth / 2 + scaleX / 2;
            foreach (var trait in _traitObjects) {
                trait.gameObject.transform.localPosition = new Vector3(offset, 0, -0.01f);
                offset += scaleX + 0.2f * scaleX;
            }
        }
        
        public void AddTrait(object trait) {
            var icon = Instantiate(AssetProvider.Instance.registry.traitIconPrefab, transform);
            var spriteRenderer = icon.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = AssetProvider.Instance.GetTraitInfo(trait.GetType()).icon;
            
            _dynamicTraits[trait.GetType()] = spriteRenderer;
            _traitObjects.Add(spriteRenderer);
            UpdatePositions();
        }
        public void AddTraits(CellStaticTraits traits) {
            uint remaining = (uint)traits;

            while (remaining != 0)
            {
                uint bit = remaining & (uint)-(int)remaining;
                CellStaticTraits flag = (CellStaticTraits)bit;

                var icon = Instantiate(AssetProvider.Instance.registry.traitIconPrefab, transform);
                var spriteRenderer = icon.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = AssetProvider.Instance.GetTraitInfo(flag).icon;
                _staticTraits[flag] = spriteRenderer;

                remaining &= ~bit;
            }
            UpdatePositions();
        }

        public void RemoveTrait(Type trait) {
            _traitObjects.Remove(_dynamicTraits[trait]);
            _dynamicTraits.Remove(trait);
            UpdatePositions();
        }

        public void RemoveTraits(CellStaticTraits traits) {
            
            uint remaining = (uint)traits;

            while (remaining != 0)
            {
                uint bit = remaining & (uint)-(int)remaining;
                CellStaticTraits flag = (CellStaticTraits)bit;

                _traitObjects.Remove(_staticTraits[flag]);
                _staticTraits.Remove(flag);

                remaining &= ~bit;
            }
            UpdatePositions();
        }
    }
}