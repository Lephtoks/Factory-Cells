using Economics;
using Entities.Navigation;

namespace Cells.Object.Building
{
    public class Drill : OneSlotBlock, INavWall, IKillable, ISquareShadow
    {
        public float MaxHealth { get; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public override bool CanReceive => false;
        public override BlockType BlockType => BlockTypes.DRILL;
        private int _counter;

        public Drill() : base() {
            MaxHealth = 200f;
            Health = MaxHealth;
        }
        
        public override void UpdateMove() {
            _counter++;
            if (_counter >= 5) {
                _counter = 0;
                if (Parent.TryGetOre(Position, out var ore)) {
                    AddItemStack(ore.OfCount(1));
                };
            }
        }
    }
}