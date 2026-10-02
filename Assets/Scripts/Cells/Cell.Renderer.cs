using System;
using System.Collections.Generic;
using System.Linq;
using Cells.Object;
using Data;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private void DamagedBlockHealthBar(IHealth health, float value) {
            if (health is not Block block || block.Parent != this) return;
            if (health.Health >= health.MaxHealth) {
                _healthBarRenderer.Remove(block);
            }
            else {
                _healthBarRenderer.Add(block, new Vector3(block.Position.x, block.Position.y, health.Health / health.MaxHealth));
            }
        }

    }
}