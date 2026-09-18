using System;
using Cells.Object;
using Core;
using Core.Locals;
using UI;
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
        private float _moveRate = 1;
        public CurrencyData CurrencyData = new();
        
        public override void Init() {
            base.Init();
            BlockTypes.Init();
            
            Cam = Camera.main;
            InfoCloud = FindGameObjectWithTag("UICloud").GetComponent<UICloudInfo>();
            Table = GameLocalBootstrap.Instance.table;
        }
        public void Update() {
            _time += Time.deltaTime;
            var t =  _time / _moveRate;
            if (t >= _moveRate * 0.85) {
                var l = (t - 0.85f) / 0.15f;
                GameLocalBootstrap.Instance.MoveEye.SetEyelidCloseState(l);
            } else if (t <= _moveRate * 0.15) {
                var l = (0.15f - t) / 0.15f;
                GameLocalBootstrap.Instance.MoveEye.SetEyelidCloseState(l);
            }
            if (_time > _moveRate) {
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