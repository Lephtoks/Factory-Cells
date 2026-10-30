using Data;
using Data.GameManagement;
using UnityEngine;

namespace Cells.Object.Building
{
    public class WindGen : Block, IDirected, IPreUpdatable, IKillable
    {
        public float MaxHealth { get; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public Direction Direction { get; set; }
        public override BlockType BlockType => BlockTypes.WIND_GEN;
        
        public WindGen() : base() {
            Direction = Direction.NORTH;
            MaxHealth = 200f;
            Health = MaxHealth;
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