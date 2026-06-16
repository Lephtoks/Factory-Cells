using Data;
using UnityEngine;

namespace Cells.Object
{
    public interface IBlockRepr
    {
        public void MakePhantom();
        public void MakeReal();
        public void MakeInvisible();
        public void SetPos(Vector2 pos);
        public void SetPos(Vector3Int pos, Transform cell);
        public void UseSettings(RepresentationSettings representationSettings);

    }
}