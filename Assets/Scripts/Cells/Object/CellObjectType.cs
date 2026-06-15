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

        public CellObject Create(Cell cell, ICellNodeRepr repr) {
            return Factory.Invoke(cell, repr);
        }
        
        private Card Instantiate() {
            Card card = UnityEngine.Object.Instantiate(AssetProvider.Instance.registry.cardPrefab);
            return card;
        }

        public Card InstantiateInShop(int index, int total) {
            Card card = Instantiate();
            card.Init(CardBehaviours.SHOP, this, index, total);
            return card;
        }

        public Card InstantiateInHand(int index, int total) {
            Card card = Instantiate();
            card.Init(CardBehaviours.HAND, this, index, total);
            return card;
        }
    };
}