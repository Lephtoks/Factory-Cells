using System;
using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Node;
using Collider;
using Data.GameManagement;
using DG.Tweening;
using Entities;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private readonly List<ICollider> _colliders = new();

        public void CheckCollisions() {
            var array = _colliders.ToArray();
            foreach (var a in array) {
                foreach (var b in array) {
                    if (a == b) continue;
                    Hitbox.Collide(a.Hitbox, b.Hitbox);
                }
            }
        }
    }
}