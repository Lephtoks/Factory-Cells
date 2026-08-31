using System;
using Cells.Object.Building;
using Cells.Object.Building.Mono;
using Core;
using Core.Asset;
using UnityEngine;

namespace Cells.Object
{
    [DefaultExecutionOrder(-1000)]
    public static class BlockTypes
    {
        private static readonly BlockAssets Blocks = AssetProvider.Instance.registry.blocks;
        public static readonly BlockType DRILL = Register(Drill.Create, Blocks.drill);
        public static readonly BlockType CONVEYOR = Register(Conveyor.Create, Blocks.conveyor);
        public static readonly BlockType WIND_GEN = Register(WindGen.Create, Blocks.windGen);
        public static readonly BlockType ITEM_SOURCE = Register(ItemSource.Create, Blocks.itemSource);

        public static BlockType Register(Func<Cell, BlockRepr, Block> creator, BlockDefinition def) {
            return new BlockType(
                creator,
                def
                );
        }
    }
}