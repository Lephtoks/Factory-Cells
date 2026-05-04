using System;
using Cells.Object;
using Core;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class CardData : MonoBehaviour
    {
        
        public CellObjectType CellObject;
        [SerializeField] private Image image;
        private Card _card;
        private void Start() {
            image.sprite = CellObject.TextureForUI;
        }

        private void Awake() {
            _card = GetComponent<Card>();
        }

        public void Activate() {
            if (_card.enabled) {
                _card.Activate();
                return;
            }
            GameStorage.Instance.AddCard(_card);
        }
    }
}