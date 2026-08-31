using System.Collections.Generic;
using Cells;
using Cells.Object;
using Core;
using Core.Locals;
using UI.Cards;
using UnityEngine;

namespace Data.Offers
{
    public class Offer
    {
        private readonly List<IOfferable> _offerableList = new List<IOfferable>();
        public Offer AddCard(BlockType cardBlockType) {
            Add(Card.Create(AssetProvider.Instance.registry.cards.cardPrefab, cardBlockType));
            return this;
        }
        
        public Offer AddCell(CellStaticTraits staticTraits, object[] dynamicTraits) {
            var offerable = Cell.Create(AssetProvider.Instance.registry.cells.cellPrefab);
            offerable.AddTrait(staticTraits);
            foreach (var dynamicTrait in dynamicTraits) {
                offerable.AddTrait(dynamicTrait);
            }
            Add(offerable);
            return this;
        }
        
        private void Add(IOfferable offerable) {
            _offerableList.Add(offerable);
        }

        public void Show() {
            GameLocalBootstrap.Instance.shopScreen.gameObject.SetActive(true);
            int column = 0;
            int totalCols = _offerableList.Count;
            foreach (var offerable in _offerableList) {
                offerable.AddToOffer(0, column, 0, totalCols);
                column += 1;
            }
        }
        public void Close() {
            foreach (var offerable in _offerableList) {
                offerable.DestroyInOffer();
            }
            GameLocalBootstrap.Instance.shopScreen.gameObject.SetActive(false);
            _offerableList.Clear();
        }

        public void SelectAndClose(IOfferable selected) {
            foreach (var offerable in _offerableList) {
                if (offerable == selected) {
                    continue;
                }

                offerable.DestroyInOffer();
            }
            selected.SelectedInOffer();
            GameLocalBootstrap.Instance.shopScreen.gameObject.SetActive(false);
            _offerableList.Clear();
        }
        public int Count => _offerableList.Count;
    }
}