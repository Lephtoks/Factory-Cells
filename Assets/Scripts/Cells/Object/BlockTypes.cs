using System;
using System.Collections.Generic;
using Cells.Object.Building;
using Core;
using Core.Asset;

namespace Cells.Object
{
    public static class BlockTypes
    {
        private static readonly BlockAssets Blocks = AssetProvider.Instance.registry.blocks;
        public static List<BlockType> Types;
        public static BlockType DRILL;
        public static BlockType CONVEYOR;
        public static BlockType WIND_GEN;
        public static BlockType ITEM_SOURCE;
        public static BlockType CELL_ANCHOR;
        public static BlockType TURRET;
        public static BlockType IMPORTER;
        public static BlockType EXPORTER;

        public static void Init() {
            Types = new List<BlockType>();
            DRILL = Register(() => new Drill(), Blocks.drill);
            CONVEYOR = Register(() => new Conveyor(), Blocks.conveyor);
            WIND_GEN = Register(() => new WindGen(), Blocks.windGen);
            ITEM_SOURCE = Register(() => new ItemSource(), Blocks.itemSource);
            CELL_ANCHOR = Register(() => new CellAnchor(), Blocks.cellAnchor);
            TURRET = Register(() => new Turret(), Blocks.turret);
            IMPORTER = Register(() => new Importer(), Blocks.importer);
            EXPORTER = Register(() => new Exporter(), Blocks.exporter);
        }
        
        public static BlockType Register(Func<Block> creator, BlockDefinition def) {
            var blockType = new BlockType(
                creator,
                def
            );
            Types.Add(blockType);
            return blockType;
        }
    }
}