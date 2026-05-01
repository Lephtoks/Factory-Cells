using System;
using Data;
using UnityEngine;

namespace Cells.Object
{
    public record CellObjectType(Func<Cell, Vector2Int, Direction, CellObject> Factory, Sprite TextureForUI, string Title, string Description);
}