using Data;
using UnityEngine.Device;

namespace Core
{
    public class GlobalManagement : Singleton<GlobalManagement>, IUpdatable
    {
        
        private int _lastWidth;
        private int _lastHeight;

        public override void Init() {
            base.Init();
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
        }

        public void Update()
        {
            if (Screen.width != _lastWidth || Screen.height != _lastHeight)
            {
                _lastWidth = Screen.width;
                _lastHeight = Screen.height;
                GameEvents.InvokeScreenSizeChange(_lastWidth, _lastHeight);
            }
        }
    }
}