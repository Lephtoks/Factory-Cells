using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class InteractionManager
    {
        private ITouchable _captured;
        private int _capturedButton = -1;
        private struct TouchableElement
        {
            public float Depth;
            public ITouchable Touchable;
        }
        private readonly List<TouchableElement> _pool =  new List<TouchableElement>();

        public void Select(Vector3 mousePos, Vector3 worldPos) {
            ITouchable hovered = null;
            if (_captured == null) {
                foreach (var element in _pool) {
                    if (!element.Touchable.IsSelected(mousePos, worldPos)) continue;
                    
                    if (_captured == null) {
                        if (Input.GetMouseButtonDown(0)) {
                            _capturedButton = 0;
                            
                            if (element.Touchable.CapturesClick()) {
                                _captured = element.Touchable;
                            }
                        }
                        else if (Input.GetMouseButtonDown(1)) {
                            _capturedButton = 1;
                            if (element.Touchable.CapturesClick()) {
                                _captured = element.Touchable;
                            }
                        }
                    }

                    hovered = element.Touchable;
                    break;
                }
            }
            
            if (_captured != null) _captured.Select(mousePos, worldPos, _capturedButton, true);
            else hovered?.Select(mousePos, worldPos, _capturedButton, false);
            
            if (_capturedButton != -1 && Input.GetMouseButtonUp(_capturedButton)) {
                _captured = null;
                _capturedButton = -1;
            }
        }
        
        
        public void Register(ITouchable touchable) {
            var touchableElement = new TouchableElement {Touchable = touchable, Depth = touchable.GetDepth()};
            SortWith(touchableElement);
            
        }
        public void Unregister(ITouchable touchable)
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i].Touchable == touchable)
                {
                    _pool.RemoveAt(i);
                    return;
                }
            }
        }

        private void SortWith(TouchableElement element)
        {
            int left = 0;
            int right = _pool.Count;

            while (left < right)
            {
                int mid = (left + right) / 2;
                
                if (_pool[mid].Depth > element.Depth)
                    left = mid + 1;
                else
                    right = mid;
            }

            _pool.Insert(left, element);
        }
        public void UpdateDepth(ITouchable touchable)
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i].Touchable == touchable)
                {
                    var element = _pool[i];
                    _pool.RemoveAt(i);

                    element.Depth = touchable.GetDepth();
                    SortWith(element);
                    return;
                }
            }
        }
    }
}