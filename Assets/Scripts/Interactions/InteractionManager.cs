using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class InteractionManager
    {
        private ITouchable _captured;
        private int _capturedButton = -1;
        private ITouchable _previousHovered;

        private struct TouchableElement
        {
            public float Depth;
            public ITouchable Touchable;
        }

        private readonly List<TouchableElement> _pool = new List<TouchableElement>();

        public bool Select(Vector3 mousePos, Vector3 worldPos)
        {
            ITouchable currentHovered = null;

            if (_captured == null)
            {
                foreach (var element in _pool)
                {
                    if (!element.Touchable.IsSelected(mousePos, worldPos))
                        continue;

                    if (Input.GetMouseButtonDown(0))
                    {
                        _capturedButton = 0;
                        if (element.Touchable.CapturesClick())
                            _captured = element.Touchable;
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        _capturedButton = 1;
                        if (element.Touchable.CapturesClick())
                            _captured = element.Touchable;
                    }

                    currentHovered = element.Touchable;
                    break;
                }
            }
            else
            {
                // Пока кнопка зажата, считаем, что курсор всё ещё находится на захваченном объекте
                currentHovered = _captured;
            }

            // Обработка смены hover-состояния
            if (currentHovered != _previousHovered)
            {
                if (_previousHovered != null)
                    _previousHovered.OnHoverEnd(mousePos, worldPos, _capturedButton);

                if (currentHovered != null)
                    currentHovered.OnHoverStart(mousePos, worldPos, _capturedButton);

                _previousHovered = currentHovered;
            }

            // Непрерывный Select (как и раньше)
            if (_captured != null)
            {
                _captured.Select(mousePos, worldPos, _capturedButton, true);
            }
            else
            {
                currentHovered?.Select(mousePos, worldPos, _capturedButton, false);
            }

            // Сброс захвата при отпускании кнопки
            if (_capturedButton != -1 && Input.GetMouseButtonUp(_capturedButton))
            {
                _captured = null;
                _capturedButton = -1;
            }

            return currentHovered != null || _captured != null;
        }

        public void Register(ITouchable touchable)
        {
            var touchableElement = new TouchableElement
            {
                Touchable = touchable,
                Depth = touchable.GetDepth()
            };
            SortWith(touchableElement);
        }

        public void Unregister(ITouchable touchable)
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                if (ReferenceEquals(_pool[i].Touchable, touchable))
                {
                    _pool.RemoveAt(i);

                    // Если удаляемый объект был в состоянии hover — завершаем его
                    if (ReferenceEquals(_previousHovered, touchable))
                    {
                        _previousHovered.OnHoverEnd(Vector3.zero, Vector3.zero, _capturedButton);
                        _previousHovered = null;
                    }

                    if (ReferenceEquals(_captured, touchable))
                    {
                        _captured = null;
                        _capturedButton = -1;
                    }

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