using Cells;
using Cells.Object;
using Cells.Object.Building;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Entities.Kinds.Mono;
using UnityEngine;

namespace Entities.Kinds
{
    public class PointEntity : Entity, IEntityRepresentable<PointRepr, PointEntity>
    {
        private Vector2 _position;
        public override Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                LivingRepresentation?.SetPos(_position);
            }
        }
        private float _angle;
        public override float Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                LivingRepresentation?.Rotate(value);
            }
        }

        public PointRepr Representation => AssetProvider.Instance.registry.pointEntity;
        public PointRepr LivingRepresentation { get; set; }
        public float DeltaPos = 0.05f;

        public PointEntity(Cell parent, Vector2 position) : base(parent, position) {
            
        }
    }
}