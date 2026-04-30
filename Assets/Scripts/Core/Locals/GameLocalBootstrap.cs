using Data;
using UI.Cloud;
using UnityEngine;

namespace Core.Locals
{
    public class GameLocalBootstrap : LocalBootstrap<GameLocalBootstrap>
    {
        protected override IUpdatable[] GetLocals() {
            return new IUpdatable[] {
                new GameStorage(),
                new MainController()
            };
        }

        public GameObject table;
        public GameObject shopScreen;

        public void OpenShopScreen() {
            shopScreen.SetActive(true);
        }

        public void CloseShopScreen() {
            shopScreen.SetActive(false);
        }
    }
}