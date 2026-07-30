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
            foreach (var a in _colliders) {
                foreach (var b in _colliders) {
                    Hitbox.Collide(a.Hitbox, b.Hitbox);
                }
            }
        }
    }
}