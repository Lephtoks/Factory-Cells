using System;
using Cells;
using Cells.Object;
using UnityEngine;

namespace Entities
{
    public record EntityKind(
        Func<Cell, Vector2, Entity> Factory,
        EntityRepr Representation,
        string Title,
        string Description)
    {

        public Entity Create(Cell cell, Vector2 pos) {
            return Factory.Invoke(cell, pos);
        }
    }
}