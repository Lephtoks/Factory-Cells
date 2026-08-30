using System;
using UnityEngine;

namespace Cells.Object
{
    public record BlockType(
        Func<Cell, BlockRepr, Block> Factory,
        BlockDefinition Def
        )
    {

    public Block Create(Cell cell, BlockRepr repr) {
        return Factory.Invoke(cell, repr);
    }
    };
}