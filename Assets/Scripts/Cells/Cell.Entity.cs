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
        
        public void AddEntity(Entity entity) => _entityList.Add(entity);
        public void RemoveEntity(Entity entity) => _entityList.Remove(entity);
        public List<Entity> GetEntities() => _entityList;

        public void UpdateEntities() {
            foreach (var entity in _entityList) {
                entity.Update();
            }
        }
    }
}