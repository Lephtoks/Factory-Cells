using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using UnityEngine;

namespace Cells.Object.Building
{
    public class WindGen : Block, IRepresentable<WindGenRepr, WindGen>, IDirected, IItemDisplayable, IPreUpdatable
    {
        public Direction Direction { get; }
        public WindGenRepr Representation => AssetProvider.Instance.registry.windGen;
        public WindGenRepr LivingRepresentation { get; set; }
        public DroppedItem DroppedItem { get; set; }
        
        public WindGen(Cell parent, Vector2Int pos, Direction direction) : base(parent, pos) {
            Direction = direction;
        }
        
        public static WindGen Create(Cell parent, WindGenRepr repr) {
            return ((IRepresentable<WindGenRepr, WindGen>)new WindGen(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y), DirectionHelper.QuaternionToDirection(repr.transform.localRotation))).AssignRepresentation(repr);
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