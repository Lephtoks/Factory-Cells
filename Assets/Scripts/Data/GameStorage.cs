using System.Collections.Generic;
using Cells.Object;
using Core;
using Core.Locals;
using UI.Cards;
using UI.Cloud;
using UnityEngine;
using static UnityEngine.GameObject;

namespace Data
{
    public class GameStorage : Singleton<GameStorage>, IUpdatable
    {
        public Card ActiveCard;
        private readonly List<Card> _cardsInHand = new();
        public Camera Cam;
        public GameObject Table;
        public UICloudInfo InfoCloud;
        private readonly List<Cell> _tilemaps = new();
        public readonly CellInventory CellInventory = new();
        private float _time;
        
        public override void Init() {
            base.Init();
            Cam = Camera.main;
            InfoCloud = FindGameObjectWithTag("UICloud").GetComponent<UICloudInfo>();
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

        public void AddCard(CellObjectType type) {
            Card instantiate = Object.Instantiate(AssetProvider.Instance.registry.cardPrefab, GameLocalBootstrap.Instance.canvasCardHolder.transform);
            instantiate.CellObject =  type;
            _cardsInHand.Add(instantiate);
            for (int i = 0; i < _cardsInHand.Count; i++) {
                _cardsInHand[i].index = i;
            }
            GameEvents.InvokeCardHandUpdate();
        }

        public void RemoveCard(CellObjectType type) {
            foreach (var cell in _cardsInHand) {
                if (cell.CellObject != type) continue;
                
                _cardsInHand.Remove(cell);
                return;
            }
            for (int i = 0; i < _cardsInHand.Count; i++) {
                _cardsInHand[i].index = i;
            }
            GameEvents.InvokeCardHandUpdate();
        }

        public List<Card> GetCards() {
            return _cardsInHand;
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