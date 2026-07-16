using System;
using UnityEngine;

namespace Cells.Object
{
    public record BlockType(
        Func<Cell, IBlockRepr, Block> Factory,
        BlockRepr Representation,
        Sprite TextureForUI,
        string Title,
        string Description)
    {

        public Block Create(Cell cell, IBlockRepr repr) {
            return Factory.Invoke(cell, repr);
        }
    };
}