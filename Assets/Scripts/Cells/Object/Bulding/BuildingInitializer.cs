using System;
using Cells.Object.Bulding.Mono;
using UnityEngine;

namespace Cells.Object.Bulding
{
    public class BuildingInitializer : MonoBehaviour
    {
        public ConveyorRepr conveyor;
        
        public static BuildingInitializer Instance { get; private set; }
        private void Awake() {
            Instance = this;
        }
    }
}