using Cells.Object.Bulding;
using Cells.Object.Bulding.Mono;
using Core;
using UnityEngine;

namespace Cells.Object
{
    public static class CellObjectTypes
    {
        public static readonly CellObjectType DRILL = new((cell, repr) => Drill.Create(cell, repr as DrillRepr), AssetProvider.Instance.registry.drill, Resources.Load<Sprite>("Textures/Items/Drill"), "Drill", "Just another drill");
        public static readonly CellObjectType CONVEYOR = new((cell, repr) => Conveyor.Create(cell, repr as ConveyorRepr), AssetProvider.Instance.registry.conveyor, Resources.Load<Sprite>("Textures/Items/Conveyor"), "Conveyor", "Just another conveyor");
    }
}