using Cells.Object.Building;
using Cells.Object.Building.Mono;
using Core;
using UnityEngine;

namespace Cells.Object
{
    public static class BlockTypes
    {
        public static readonly BlockType DRILL = new((cell, repr) => Drill.Create(cell, repr as DrillRepr), AssetProvider.Instance.registry.drill, Resources.Load<Sprite>("Textures/Items/Drill"), "Drill", "Just another drill");
        public static readonly BlockType CONVEYOR = new((cell, repr) => Conveyor.Create(cell, repr as ConveyorRepr), AssetProvider.Instance.registry.conveyor, Resources.Load<Sprite>("Textures/Items/Conveyor"), "Conveyor", "Just another conveyor");
    }
}