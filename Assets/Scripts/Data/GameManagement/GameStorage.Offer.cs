using System.Collections.Generic;
using Cells.Object;
using Core.Locals;
using Data.Offers;
using DG.Tweening;
using UI.Cards;
using UnityEngine;

namespace Data.GameManagement
{
    public partial class GameStorage
    {
        private Offer _currentOffer;
    
        private readonly List<Card> _allOfferedCards = new List<Card>();

        public Offer AddOffer() {
            var offer = new Offer();
            _currentOffer = offer;
            return offer;
        }
        
        public Offer GetOffer() {
            return _currentOffer;
        }

        
        public void MoveOnOfferLayer(GameObject gameObject) {
            gameObject.layer = LayerMask.NameToLayer("ObjectInOffer");
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                Transform child = gameObject.transform.GetChild(i);
                MoveOnOfferLayer(child.gameObject);
            }
        }

        public void MoveOnDefaultLayer(GameObject gameObject) {
            gameObject.layer = LayerMask.NameToLayer("Default");
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                Transform child = gameObject.transform.GetChild(i);
                MoveOnDefaultLayer(child.gameObject);
            }
        }
    }
}