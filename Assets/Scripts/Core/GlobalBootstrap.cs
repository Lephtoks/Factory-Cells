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
        
        private void RegisterLocals() {
            _locals.Add(BootstrapsIdentity.GAMEPLAY, new IUpdatable[] {
                new GameStorage(),
                new MainController()
            });
        }
        private readonly Dictionary<BootstrapsIdentity, IUpdatable[]> _locals = new();
    
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(this);
                return;
            };
            foreach (var g in _globals) {
                g.Init();
            }
            RegisterLocals();
            
            Instance = this;
        }

        public IUpdatable[] GetLocals(BootstrapsIdentity identity) {
            return _locals[identity];
        }
    }
}