using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Economics;
using Entities.Navigation;
using GameRenderer;
using UnityEngine;

namespace Cells.Object.Building
{
    public class Drill : OneSlotBlock, INavWall, IKillable, ISquareShadow
    {
        public float MaxHealth { get; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public override bool CanReceive => false;
        public override BlockType BlockType => BlockTypes.DRILL;
        public DefaultRepr LivingRepresentation { get; set; }
        private int _counter;

        public Drill(Cell parent) : base(parent) {
            MaxHealth = 200f;
            Health = MaxHealth;
        }
        
        public override void UpdateMove() {
            _counter++;
            if (_counter >= 5) {
                _counter = 0;
                if (Parent.TryGetOre(Position, out var ore)) {
                    AddItemStack(ore.OfCount(1));
                };
            }
        }

        public static Block Create(Cell parent) {
            return new Drill(parent);
        }
    }
}