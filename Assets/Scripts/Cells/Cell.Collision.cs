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

            for (int i = 0; i < _colliders.Count; i++) {
                var colliderA = _colliders[i];
                for (int j = i + 1; j < _colliders.Count; j++) {
                    var colliderB = _colliders[j];
                    Hitbox.Collide(colliderA.Hitbox, colliderB.Hitbox);
                }
            }
        }
    }
}