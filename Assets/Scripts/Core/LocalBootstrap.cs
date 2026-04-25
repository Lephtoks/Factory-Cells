using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-999)]
    public class LocalBootstrap : MonoBehaviour
    {
        public BootstrapsIdentity identity;
        private IUpdatable[] _locals;
        private void Awake() {
            _locals = GlobalBootstrap.Instance.GetLocals(identity);
            foreach (var l in _locals) {
                l.Init();
            }
        }

        private void Update() {
            foreach (var l in _locals) {
                l.Update();
            }
        }
    }
}