using Core;
using Core.Locals;
using UI.Cloud;
using UnityEngine;
using static UnityEngine.GameObject;

namespace Data.GameManagement
{
    public partial class GameStorage : Singleton<GameStorage>, IUpdatable
    {
        public Camera Cam;
        public GameObject Table;
        public UICloudInfo InfoCloud;
        private float _time;
        public CurrencyData CurrencyData = new();
        
        public override void Init() {
            base.Init();
            Cam = Camera.main;
            InfoCloud = FindGameObjectWithTag("UICloud").GetComponent<UICloudInfo>();
            Table = GameLocalBootstrap.Instance.table;
        }
        public void Update() {
            _time += Time.deltaTime;
            if (_time > 1f) {
                CurrencyData.Wind = 0;
                foreach (var cell in _tilemaps) {
                    cell.UpdatePreMove();
                }
                foreach (var cell in _tilemaps) {
                    cell.UpdateMove();
                }
                _time = 0;
            }
        }
    }
}