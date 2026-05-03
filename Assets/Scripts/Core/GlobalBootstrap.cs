using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-1000)]
    public class GlobalBootstrap : MonoBehaviour
    {
        public static GlobalBootstrap Instance { get; private set; }
        private readonly IBootable[] _globals = {
            new AssetProvider(),
            new GameDataManager()
        };
    
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(this);
                return;
            };
            foreach (var g in _globals) {
                g.Init();
            }
            Instance = this;
        }

        private void Update() {
            foreach (var g in _globals) {
                if (g is IUpdatable updatable) {
                    updatable.Update();
                }
            }
        }
    }
}