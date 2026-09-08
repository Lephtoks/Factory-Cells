using System;
using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Node;
using Data.GameManagement;
using DG.Tweening;
using Entities;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private readonly List<Entity> _entityList = new();
        
        public void AddEntity(Entity entity) {
            _entityList.Add(entity);
            _colliders.Add(entity);
            if (entity is IEntityRepresentable representable) {
                representable.Represent(entity);
            }
        }
        public void RemoveEntity(Entity entity) {
            _entityList.Remove(entity);
            _colliders.Remove(entity);
        }
        public List<Entity> GetEntities() => _entityList;

        public void UpdateEntities() {
            for (int i = 0; i < _entityList.Count; i++)
            {
                var entityA = _entityList[i];
                for (int j = i + 1; j < _entityList.Count; j++)
                {
                    var entityB = _entityList[j];
                    
                    Vector2 delta = entityA.Position - entityB.Position;
                    float radius = entityA.RepulsionRadius + entityB.RepulsionRadius;

                    if (delta.sqrMagnitude <= radius * radius) {
                        entityA.Repulse(entityB);
                        entityB.Repulse(entityA);
                    }
                }
            }
            foreach (var entity in _entityList) {
                entity.Update();
            }
        }

        public void UpdateAliveness() {
            foreach (var entity in _entityList.ToArray()) {
                if (entity.Dead) {
                    RemoveEntity(entity);
                }
            }
        }
    }
}