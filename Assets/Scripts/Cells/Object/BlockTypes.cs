using System;
using Cells.Object.Building;
using Cells.Object.Building.Mono;
using Core;
using UnityEngine;

namespace Cells.Object
{
    public static class BlockTypes
    {
        public static readonly BlockType DRILL = Register(Drill.Create, AssetProvider.Instance.registry.drill);
        public static readonly BlockType CONVEYOR = Register(Conveyor.Create, AssetProvider.Instance.registry.conveyor);
        public static readonly BlockType WIND_GEN = Register(WindGen.Create, AssetProvider.Instance.registry.windGen);
        public static readonly BlockType ITEM_SOURCE = Register(ItemSource.Create, AssetProvider.Instance.registry.itemSource);

        public static BlockType Register(Func<Cell, BlockRepr, Block> creator, BlockDefinition def) {
            return new BlockType(
                creator,
                def
                );
        }
    }
}