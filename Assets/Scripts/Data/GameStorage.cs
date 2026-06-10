using System;
using System.Collections.Generic;
using Cells.Object;
using Core;
using Core.Locals;
using UI.Cards;
using UI.Cloud;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GameObject;
using Object = UnityEngine.Object;

namespace Data
{
    public class GameStorage : Singleton<GameStorage>, IUpdatable
    {
        public Card ActiveCard {private set; get;}
        private readonly List<Card> _cardsInHand = new();
        public Camera Cam;
        public GameObject Table;
        public UICloudInfo InfoCloud;
        private readonly List<Cell> _tilemaps = new();
        public readonly CellInventory CellInventory = new();
        private float _time;
        public readonly List<CellNodeRepr> NodeReprs = new();
        public readonly RepresentationSettings RepresentationSettings = new();
        private Dictionary<System.Type, List<CellNodeRepr>> _reprCache = new();
        
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

        public void AddCard(Card card) {
            _cardsInHand.Add(card);
            for (int i = 0; i < _cardsInHand.Count; i++) {
                _cardsInHand[i].index = i;
            }
            GameEvents.InvokeCardHandUpdate();
        }

        public void RemoveCard(CellObjectType type) {
            foreach (var cell in _cardsInHand) {
                if (cell.CellObject != type) continue;
                
                _cardsInHand.Remove(cell);
                Object.Destroy(cell.gameObject);
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

        public void SetActiveCard(Card card) {
            ActiveCard = card;
            UpdatePointerRepr();
        }

        public void UpdatePointerRepr() {
            foreach (var repr in NodeReprs.ToArray()) {
                RemoveRepresentation(repr);
            }
            CreatePointerRepr();
            
        }

        public void CreatePointerRepr() {
            CellNodeRepr cellNodeRepr = CreateRepresentation(ActiveCard.CellObject.Representation);
            cellNodeRepr.MakeInvisible();
        }
        public void SetAmountOfRepresentations(CellNodeRepr cellObjectRepresentation, int reprs) {
            int visibleCount = 0;
            System.Type targetType = cellObjectRepresentation.GetType();

            foreach (var repr in NodeReprs.ToArray()) {
                if (repr.GetType() != targetType) {
                    RemoveRepresentation(repr);
                    continue;
                }

                if (visibleCount < reprs) {
                    repr.MakePhantom();
                    repr.UseSettings(RepresentationSettings);
                    visibleCount++;
                }
                else {
                    RemoveRepresentation(repr);
                }
            }

            for (int i = visibleCount; i < reprs; i++) {
                var repr = CreateRepresentation(cellObjectRepresentation);
                repr.MakePhantom();
                repr.UseSettings(RepresentationSettings);
            }
        }

        public CellNodeRepr CreateRepresentation(CellNodeRepr cellObjectRepresentation) {
            CellNodeRepr repr;
            Type type = cellObjectRepresentation.GetType();
            if (_reprCache.TryGetValue(type, out List<CellNodeRepr> reprs)) {
                if (reprs.Count > 0) {
                    repr = reprs[^1];
                    reprs.RemoveAt(reprs.Count - 1);
                    NodeReprs.Add(repr);
                    return repr;
                }
            }
            else {
                _reprCache[type] = new List<CellNodeRepr>();
            }
            repr = Object.Instantiate(cellObjectRepresentation);
            NodeReprs.Add(repr);
            return repr;
        }

        public void RemoveRepresentation(CellNodeRepr cellObjectRepresentation) {
            
            Type type = cellObjectRepresentation.GetType();
            if (!_reprCache.TryGetValue(type, out List<CellNodeRepr> reprs)) {
                _reprCache[type] = reprs = new List<CellNodeRepr>();
            }
            NodeReprs.Remove(cellObjectRepresentation);
            if (reprs.Count < 5) {
                reprs.Add(cellObjectRepresentation);
                cellObjectRepresentation.MakeInvisible();
                return;
            }
            Object.Destroy(cellObjectRepresentation.gameObject);
            
        }
    }
}