using System.Collections.Generic;
using System.Linq;
using Data;

namespace Cells
{
    public partial class Cell
    {
        private readonly Dictionary<Direction, int> _windSides = new();
        
        public int GetWind() {
            return _windSides.Values.Count == 0 ? 0 : _windSides.Values.Max();
        }

        public void ResetWind() {
            _windSides.Clear();
        }

        public void BlowWind(Direction direction, int power) {
            _windSides[direction] = _windSides.TryGetValue(direction, out int windValue) ? (windValue + power) : power;
        }

    }
}