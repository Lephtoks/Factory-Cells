using System;
using UnityEngine;

namespace Cells.Object
{
    public record BlockType(
        Func<Cell, Block> Factory,
        BlockDefinition Def
        )
    {

    public Block Create(Cell cell) {
        return Factory.Invoke(cell);
    }
    };
}