using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Data.GameManagement;
using Economics;
using UnityEngine;

namespace Cells.Object.Building
{
    public class WindGen : Block, IRepresentable<WindGenRepr>, IDirected, IItemDisplayable, IPreUpdatable
    {
        public Direction Direction { get; }
        public override BlockType BlockType => BlockTypes.WIND_GEN;
        public WindGenRepr LivingRepresentation { get; set; }
        public DroppedItem DroppedItem { get; set; }
        
        public WindGen(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos) {
            Direction = direction;
        }
        
        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<WindGenRepr>)new WindGen(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y), DirectionHelper.QuaternionToDirection(repr.transform.localRotation))).AssignRepresentation(repr);
        }

        public void UpdatePreMove() {
            var dir = Direction.ToVector2Int();
            var pos = Position;
            while (Parent.tilemap.HasTile((Vector3Int) pos)) {
                pos += dir;
                if (!Parent.IsTileEmpty(pos)) return;
            }

            GameStorage.Instance.CurrencyData.Wind += 3;
        }
    }
}