using Cells;
using Cells.Object;
using Data;
using Data.GameManagement;
using Economics;
using UI;
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
        public GameObject CellHolder;
        public GameObject canvasCardHolder;
        public GameObject RepresenterStore;
        public SelectionZone SelectionZone;
        public MoveEye MoveEye;
        public DayClock DayClock;

        public void OpenOffer() {
            GameStorage.Instance.AddOffer()
                .AddCard(BlockTypes.DRILL) 
                .AddCard(BlockTypes.CONVEYOR)
                .AddCard(BlockTypes.WIND_GEN)
                .AddCard(BlockTypes.TURRET)
                // .AddCell()
                .AddCell(CellStaticTraits.NONE, new [] { CellDynamicTraits.AttackTrait() }, new CellArguments(new []{ new OreData { OreAmount = 6, OreType = Currency.COPPER_ORE}, new OreData { OreAmount = 7, OreType = Currency.STONE}}))
                .Show();
        }
    }
}