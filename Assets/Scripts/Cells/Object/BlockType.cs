using System;
using UnityEngine;

namespace Cells.Object
{
    public record BlockType(
        Func<Block> Factory,
        BlockDefinition Def
        )
    {

        public Block Create() {
            return Factory.Invoke();
        }
    }
}