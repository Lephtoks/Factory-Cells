using Core;
using UnityEngine;

namespace UI.Shop
{
    public class ShopScreen : MonoSingleton<ShopScreen>
    {
        public RectTransform rectTransform;
        public override void Awake() {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
        }
    }
}