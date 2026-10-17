using System.Collections.Generic;
using GameRenderer;
using UnityEngine;

namespace Cells.Object
{
    public interface IShadow
    {
        IEnumerable<ShadowRect> CreateShadow();
        int ShadowCount { get; }
    }
    public interface ISquareShadow : IShadow, IPositioned {
        IEnumerable<ShadowRect> IShadow.CreateShadow() {
            yield return new ShadowRect(Position, Vector2.one, 0.25f);
        }

        int IShadow.ShadowCount => 1;
    }
}