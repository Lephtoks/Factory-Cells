using System;
using Core;
using Core.Locals;
using Data;
using UI.Cards;
using UnityEngine;

namespace Cells.Object
{
    public record CellObjectType(
        Func<Cell, Vector2Int, Direction, CellObject> Factory,
        Sprite TextureForUI,
        string Title,
        string Description)
    {
        public Card Instantiate() {
            Card instantiate = UnityEngine.Object.Instantiate(AssetProvider.Instance.registry.cardPrefab);
            instantiate.Data.CellObject = this;
            return instantiate;
        }
    };
}