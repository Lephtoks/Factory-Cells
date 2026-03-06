using System;
using System.Collections.Generic;
using UI.Cards;
using UI.Cloud;
using UnityEngine;

namespace Data
{
    [DefaultExecutionOrder(-1000)]
    public class GameStorage : MonoBehaviour
    {
        public static GameStorage Instance;

        private void Awake() {
            Instance = this;
            cam = Camera.main;
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
        private List<Cell> tilemaps = new();
        public Camera cam;
        public GameObject Table;
        public CellInventory CellInventory = new CellInventory();
        public UICloudInfo InfoCloud;


        private float time = 0;
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