using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-999)]
    public abstract class LocalBootstrap<T> : MonoSingleton<T> where T : class 
    {
        private IUpdatable[] _locals;

        public override void Awake() {
            base.Awake();
            _locals = GetLocals();
            foreach (var l in _locals) {
                l.Init();
            }
        }

        protected abstract IUpdatable[] GetLocals();

        private void Update() {
            foreach (var l in _locals) {
                l.Update();
            }
        }
    }
}