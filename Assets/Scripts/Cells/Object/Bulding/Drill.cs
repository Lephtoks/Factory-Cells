using Cells.Object.Bulding.Mono;
using Core;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Bulding
{
    public class Drill : OneSlotCellNode<DrillRepr, Drill>
    {

        public Drill(Cell parent, DrillRepr repr) : base(parent, repr) {
        }
        
        private int _counter;
        public override void UpdateMove() {
            _counter++;
            if (_counter >= 5) {
                _counter = 0;
                AddItemStack(Currency.COPPER.OfCount(1));
                Debug.Log(GetItemStack().Count);
                Debug.Log(GetItemStack().CurrencyType);
            }
        }
    }
}