using System.Collections.Generic;
using GameRenderer;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Turret: Block, IKillable, IShadow
    {
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public override BlockType BlockType => BlockTypes.TURRET;
        public Vector2 TargetPosition { get; set; }
        public bool Targeted;
        
        public Turret() : base() {
            MaxHealth = 500;
            Health = 500;
        }

        public override void UpdateMove() {
            base.UpdateMove();
            if (Targeted) {
                // TODO
                // Parent.AddBullet(new Bullet(Parent,
                //     Parent.CellPivot.InverseTransformPoint(LivingRepresentation.BulletCreatorTransform.position),
                //     LivingRepresentation.TowerTransform.localRotation.eulerAngles.z + 180, BulletTypes.DEFAULT));
                // LivingRepresentation.Shot();
            }
        }
        

        public IEnumerable<ShadowRect> CreateShadow() {
            yield return new ShadowRect(Position, new Vector2(1, 15/16f), 0.25f);
            yield return new ShadowRect(Position + new Vector2(1/16f, 15/16f), new Vector2(14/16f, 1/16f), 0.25f);
        }

        public int ShadowCount => 2;
    }
}