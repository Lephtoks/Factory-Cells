using System;
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
    public class PointEntity : Entity, IEntityRepresentable<PointRepr, PointEntity>, IDisposable
    {
        private Vector2 _position;
        private float _attackTime;
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

        public PointRepr Representation => AssetProvider.Instance.registry.entities.pointEntity;
        public PointRepr LivingRepresentation { get; set; }
        public float DeltaPos = 0.05f;

        public PointEntity(Cell parent, Vector2 position) : base(parent, position) {
            Parent.NavTree.NavTreeRebuildEvent += PathChanged;
            PathChanged();
        }

        public override void Update() {
            base.Update();
            if (!LivingRepresentation) return;
            _attackTime += Time.deltaTime;
            if (_attackTime >= 0.5f) {
                Vector2Int la = (Vector2Int) Parent.tilemap.WorldToCell(LivingRepresentation.LeftArmAttackPoint.transform.position);
                Vector2Int ra = (Vector2Int) Parent.tilemap.WorldToCell(LivingRepresentation.RightArmAttackPoint.transform.position);

                if (Parent.TryGetObject(la, out Block block) && block is Cells.Object.IHealth health) {
                    health.Damage(25);
                }
                if (Parent.TryGetObject(ra, out Block block2) && block2 is Cells.Object.IHealth health2) {
                    health2.Damage(25);
                }
                _attackTime = 0;
            }
        }

        public void Dispose() {
            Parent.NavTree.NavTreeRebuildEvent -= PathChanged;
        }

        public void PathChanged() {
            Target = Parent.NearestBlock(Position).Position + Vector2.one * 0.5f;
            Pathfind();
        }
    }
}