using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using Entities.Navigation;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Drill : OneSlotBlock, IRepresentable<DefaultRepr>, INavWall
    {
        public override BlockType BlockType => BlockTypes.DRILL;
        public DefaultRepr LivingRepresentation { get; set; }
        private int _counter;

        public Drill(Cell parent, Vector2Int pos) : base(parent, pos) {
        }
        
        public override IEnumerable<Direction> OutDirections() {
            yield return Direction.EAST;
            yield return Direction.WEST;
            yield return Direction.NORTH;
            yield return Direction.SOUTH;
        }


        public override void UpdateMove() {
            _counter++;
            if (_counter >= 5) {
                _counter = 0;
                AddItemStack(Currency.COPPER.OfCount(1));
                Debug.Log(GetItemStack().Count);
                Debug.Log(GetItemStack().CurrencyType);
            }
        }

        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<DefaultRepr>) new Drill(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y))).AssignRepresentation(repr);
        }
    }
}