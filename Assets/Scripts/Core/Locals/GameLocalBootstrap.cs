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
            GameStorage.Instance.AddOffer()
                .AddCard(BlockTypes.ITEM_SOURCE)
                .AddCard(BlockTypes.DRILL)
                .AddCard(BlockTypes.CONVEYOR)
                .AddCard(BlockTypes.WIND_GEN)
                .Show();
        }
    }
}