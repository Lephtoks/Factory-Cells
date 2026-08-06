using System;
using System.Collections.Generic;
using Data;
using GameDebug;
using UnityEngine;
using UnityEngine.Rendering;
using DebugManager = GameDebug.DebugManager;

namespace Core
{
    [DefaultExecutionOrder(-1000)]
    public class GlobalBootstrap : MonoBehaviour
    {
        public static GlobalBootstrap Instance { get; private set; }
        private readonly IBootable[] _globals = {
            new AssetProvider(),
            new GameDataManager(),
            new DebugManager()
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

        private void OnDestroy() {
            foreach (var g in _globals) {
                g.Dispose();
            }
        }
    }
}