using System;
using Core;
using Core.Locals;
using Data;
using UI.Cards;
using UnityEngine;

namespace Cells.Object
{
    public record CellObjectType(
        Func<Cell, ICellNodeRepr, CellObject> Factory,
        CellNodeRepr Representation,
        Sprite TextureForUI,
        string Title,
        string Description)
    {
        
        private Card Instantiate(CellObjectType type) {
            Card instantiate = UnityEngine.Object.Instantiate(AssetProvider.Instance.registry.cardPrefab);
            return instantiate;
        }

        public Card InstantiateInShop(CellObjectType type, int index, int total) {
            Card ins = Instantiate(type);
            ins.Init(CardBehaviours.SHOP, this, index, total);
            return ins;
        }

        public Card InstantiateInHand(CellObjectType type, int index, int total) {
            Card ins = Instantiate(type);
            ins.Init(CardBehaviours.HAND, this, index, total);
            return ins;
        }
    };
}