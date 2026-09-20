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
        public CurrencyData CurrencyData = new();
        public readonly GameStorageTime Time = new();
        
        public override void Init() {
            base.Init();
            BlockTypes.Init();
            
            Cam = Camera.main;
            InfoCloud = FindGameObjectWithTag("UICloud").GetComponent<UICloudInfo>();
            Table = GameLocalBootstrap.Instance.table;
        }
        public void Update() {
            Time.MoveTime += UnityEngine.Time.deltaTime;
            var t =  Time.MoveTime / Time.MoveRate;
            switch (GameLocalBootstrap.Instance.MoveEye.IsClosing) {
                case false when t >= Time.MoveRate * 0.85:
                    GameLocalBootstrap.Instance.MoveEye.Close();
                    break;
                case true when t <= Time.MoveRate * 0.15:
                    GameLocalBootstrap.Instance.MoveEye.Open();
                    break;
            }
            if (Time.MoveTime > Time.MoveRate) {
                Time.DayTime += 1;
                GameLocalBootstrap.Instance.DayClock.UpdatePointer();
                if (Time.DayTime >= Time.HoursInDay) {
                    Time.DayTime = 0;
                    Time.Day++;
                }
                CurrencyData.Wind = 0;
                foreach (var cell in _tilemaps) {
                    cell.UpdatePreMove();
                }
                foreach (var cell in _tilemaps) {
                    cell.UpdateMove();
                }
                Time.MoveTime = 0;
            }
        }
    }

    public class GameStorageTime
    {
        public float MoveTime;
        public float MoveRate = 1;
        public int DayTime;
        public int HoursInDay = 24;
        public int Day;
    }
}