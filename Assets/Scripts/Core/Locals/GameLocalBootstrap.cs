using Cells.Object;
using Data;
using Data.GameManagement;
using UI.Cards;
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
        public CardHolder canvasCardHolder;

        public void OpenOffer() {
            MainController.Instance.
                ShowOffer(
                    BlockTypes.ITEM_SOURCE,
                    BlockTypes.DRILL,
                    BlockTypes.CONVEYOR,
                    BlockTypes.WIND_GEN
                );
        }
    }
}