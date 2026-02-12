using UnityEngine;

namespace Cells.Object
{
    public abstract class CellNodeRepr<T> : MonoBehaviour where T : CellObject
    {
        public Texture2D textureForUI;
        public abstract void Init(T original);
    }
}