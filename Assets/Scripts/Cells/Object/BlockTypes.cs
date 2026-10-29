using System;
using System.Collections.Generic;
using Cells.Object.Building;
using Cells.Object.Building.Mono;
using Core;
using Core.Asset;
using UnityEngine;

namespace Cells.Object
{
    public static class BlockTypes
    {
        private static readonly BlockAssets Blocks = AssetProvider.Instance.registry.blocks;
        public static BlockType DRILL;
        public static BlockType CONVEYOR;
        public static BlockType WIND_GEN;
        public static BlockType ITEM_SOURCE;
        public static BlockType CELL_ANCHOR;
        public static BlockType TURRET;
        public static BlockType IMPORTER;
        public static BlockType EXPORTER;

        public static void Init() {
            DRILL = Register(Drill.Create, Blocks.drill);
            CONVEYOR = Register(Conveyor.Create, Blocks.conveyor);
            WIND_GEN = Register(WindGen.Create, Blocks.windGen);
            ITEM_SOURCE = Register(ItemSource.Create, Blocks.itemSource);
            CELL_ANCHOR = Register(CellAnchor.Create, Blocks.cellAnchor);
            TURRET = Register(Turret.Create, Blocks.turret);
            IMPORTER = Register(Importer.Create, Blocks.importer);
            EXPORTER = Register(Exporter.Create, Blocks.exporter);
        }
        
        public static BlockType Register(Func<Cell, Block> creator, BlockDefinition def) {
            var blockType = new BlockType(
                creator,
                def
            );
            return blockType;
        }
    }
}