using System;
using Data;
using UnityEngine;

namespace Cells.Object
{
    public record CellObjectType(Func<Cell, Vector2Int, Direction, CellObject> factory, Texture2D textureForUI);
}