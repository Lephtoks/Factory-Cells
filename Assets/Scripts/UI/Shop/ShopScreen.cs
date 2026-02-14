using UnityEngine;

namespace UI.Shop
{
    public class ShopScreen : MonoBehaviour
    {
        public static ShopScreen Instance;

        private void Awake() {
            Instance = this;
            this.gameObject.SetActive(false);
        }
    }
}