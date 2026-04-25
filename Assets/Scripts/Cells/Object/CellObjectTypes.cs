using Cells.Object.Bulding;
using UnityEngine;

namespace Cells.Object
{
    public static class CellObjectTypes
    {
        public static readonly CellObjectType DRILL = new((cell, pos, dir) => new Drill(cell, pos, dir), Resources.Load<Sprite>("Textures/Items/Drill"));
        public static readonly CellObjectType CONVEYOR = new((cell, pos, dir) => new Conveyor(cell, pos, dir), Resources.Load<Sprite>("Textures/Items/Conveyor"));
    }
}