using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using Entities;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Turret: Block, IRepresentable<DefaultRepr>, IKillable
    {
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public override BlockType BlockType => BlockTypes.TURRET;
        public DefaultRepr LivingRepresentation { get; set; }
        
        public Turret(Cell parent, Vector2Int pos) : base(parent, pos) {
            MaxHealth = 500;
            Health = 500;
        }

        public override void UpdateMove() {
            base.UpdateMove();
            Parent.AddBullet(new Bullet(Parent, Position, 0, BulletTypes.DEFAULT));
        }

        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<DefaultRepr>) new Turret(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y))).AssignRepresentation(repr);
        }
    }
}