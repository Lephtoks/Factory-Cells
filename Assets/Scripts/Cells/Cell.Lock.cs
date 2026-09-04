using System;
using System.Collections.Generic;
using Cells.Object;
using Core.Locals;
using Data;
using Data.GameManagement;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private readonly List<ICellLocker> _lockers = new List<ICellLocker>();

        public void Lock(ICellLocker locker) {
            _lockers.Add(locker);
        }

        public void Unlock(ICellLocker locker) {
            _lockers.Remove(locker);
            if (_behaviour == CellBehaviours.INVENTORY || _behaviour == CellBehaviours.TABLE) {
                GameStorage.Instance.CellInventory.UpdateTableLocks();
            }
        }

        public bool IsLocked() {
            return _lockers.Count != 0;
        }
    }
}