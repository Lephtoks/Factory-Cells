using Core;
using Entities.Kinds;
using Entities.Kinds.Mono;
using UnityEngine;

namespace Entities
{
    public class EntityKinds
    {
        public static readonly EntityKind POINT = new((cell, pos) => new PointEntity(cell, pos), AssetProvider.Instance.registry.pointEntity, "Point", "Just another point");
    }
}