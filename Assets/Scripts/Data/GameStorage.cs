using System.Collections.Generic;
using Core;
using Core.Locals;
using UI.Cards;
using UI.Cloud;
using UnityEngine;

namespace Data
{
    public class GameStorage : Singleton<GameStorage>, IUpdatable
    {
        public Card ActiveCard;
        public Camera Cam;
        public GameObject Table;
        public UICloudInfo InfoCloud;
        private readonly List<Cell> _tilemaps = new();
        public readonly CellInventory CellInventory = new();
        private float _time;
        
        public override void Init() {
            base.Init();
            Cam = Camera.main;
            InfoCloud = GameObject.FindGameObjectWithTag("UICloud").GetComponent<UICloudInfo>();
            Table = GameLocalBootstrap.Instance.table;
        }

        public void AddCell(Cell cell) {
            _tilemaps.Add(cell);
            CellInventory.AddCell(cell);
        }

        public void RemoveCell(Cell cell) {
            _tilemaps.Remove(cell);
            CellInventory.RemoveCell(cell);
        }

        public IReadOnlyList<Cell> GetCells() {
            return _tilemaps;
        }

        public void Update() {
            _time += Time.deltaTime;
            if (_time > 1f) {
                foreach (var cell in _tilemaps) {
                    cell.UpdateMove();
                }
                _time = 0;
            }
        }
    }
}