using System;
using Cells.Object;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class CardData : MonoBehaviour
    {
        
        public CellObjectType CellObject;
        [SerializeField] private Image image;
        private void Start() {
            image.sprite = CellObject.TextureForUI;
        }
    }
}