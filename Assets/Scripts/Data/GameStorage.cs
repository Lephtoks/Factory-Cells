using System.Collections.Generic;
using Core;
using UI.Cards;
using UI.Cloud;
using UnityEngine;

namespace Data
{
    public class GameStorage : Singleton<GameStorage>, IUpdatable
    {
        public override void Init() {
            base.Init();
            cam = Camera.main;
            InfoCloud = GameObject.FindGameObjectWithTag("UICloud").GetComponent<UICloudInfo>();
            Table = GameObject.FindGameObjectWithTag("Table");
        }

        public void AddCell(Cell cell) {
            tilemaps.Add(cell);
            CellInventory.AddCell(cell);
        }

        public void RemoveCell(Cell cell) {
            tilemaps.Remove(cell);
            CellInventory.RemoveCell(cell);
        }

        public IReadOnlyList<Cell> GetCells() {
            return tilemaps;
        }
        
        public Card ActiveCard;
        public Camera cam;
        public GameObject Table;
        public UICloudInfo InfoCloud;
        
        private List<Cell> tilemaps = new();
        public CellInventory CellInventory = new();


        private float time;
        public void Update() {
            time += Time.deltaTime;
            if (time > 1f) {
                foreach (var cell in tilemaps) {
                    cell.UpdateMove();
                }
                time = 0;
            }
        }
    }
}