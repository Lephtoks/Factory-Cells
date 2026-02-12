using Cells.Object.Bulding;
using UnityEngine;

namespace Cells.Object
{
    public static class CellObjectTypes
    {
        public static readonly CellObjectType CONVEYOR = new CellObjectType((cell, pos, dir) => new Conveyor(cell, pos, dir), Resources.Load<Texture2D>("Textures/Items/Conveyor"));
    }
}