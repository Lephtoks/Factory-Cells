using Core;

namespace UI.Shop
{
    public class ShopScreen : MonoSingleton<ShopScreen>
    {
        public override void Awake() {
            base.Awake();
            gameObject.SetActive(false);
        }
    }
}