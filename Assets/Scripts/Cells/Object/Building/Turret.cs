using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using Entities;
using GameRenderer;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Turret: Block, IRepresentable<TurretRepr>, IKillable, IShadow
    {
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public override BlockType BlockType => BlockTypes.TURRET;
        public TurretRepr LivingRepresentation { get; set; }
        public Vector2 TargetPosition { get; set; }
        public bool Targeted;
        
        public Turret(Cell parent, Vector2Int pos) : base(parent, pos) {
            MaxHealth = 500;
            Health = 500;
        }

        public override void UpdateMove() {
            base.UpdateMove();
            if (Targeted) {
                Parent.AddBullet(new Bullet(Parent,
                    Parent.CellPivot.InverseTransformPoint(LivingRepresentation.BulletCreatorTransform.position),
                    LivingRepresentation.TowerTransform.localRotation.eulerAngles.z + 180, BulletTypes.DEFAULT));
                LivingRepresentation.Shot();
            }
        }


        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<TurretRepr>) new Turret(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y))).AssignRepresentation(repr);
        }
        

        public IEnumerable<ShadowRect> CreateShadow() {
            yield return new ShadowRect(Position, new Vector2(1, 15/16f), 0.25f);
            yield return new ShadowRect(Position + new Vector2(1/16f, 15/16f), new Vector2(14/16f, 1/16f), 0.25f);
        }

        public int ShadowCount => 2;
    }
}