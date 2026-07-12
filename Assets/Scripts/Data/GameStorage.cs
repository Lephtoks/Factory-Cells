using System;
using System.Collections.Generic;
using Cells;
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
        public readonly List<BlockRepr> NodeReprs = new();
        public readonly RepresentationSettings RepresentationSettings = new();
        private readonly Dictionary<System.Type, List<BlockRepr>> _reprCache = new();
        public CurrencyData CurrencyData = new();
        
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

        public void RemoveCard(BlockType type) {
            foreach (var cell in _cardsInHand) {
                if (cell.Block != type) continue;
                
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
            BlockRepr blockRepr = CreateRepresentation(ActiveCard.Block.Representation);
            blockRepr.MakeInvisible();
        }
        public void SetAmountOfRepresentations(BlockRepr cellObjectRepresentation, int reprs) {
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

        public BlockRepr CreateRepresentation(BlockRepr cellObjectRepresentation) {
            BlockRepr repr;
            Type type = cellObjectRepresentation.GetType();
            if (_reprCache.TryGetValue(type, out List<BlockRepr> reprs)) {
                if (reprs.Count > 0) {
                    repr = reprs[^1];
                    reprs.RemoveAt(reprs.Count - 1);
                    NodeReprs.Add(repr);
                    return repr;
                }
            }
            else {
                _reprCache[type] = new List<BlockRepr>();
            }
            repr = Object.Instantiate(cellObjectRepresentation);
            NodeReprs.Add(repr);
            return repr;
        }

        public void RemoveRepresentation(BlockRepr cellObjectRepresentation) {
            
            Type type = cellObjectRepresentation.GetType();
            if (!_reprCache.TryGetValue(type, out List<BlockRepr> reprs)) {
                _reprCache[type] = reprs = new List<BlockRepr>();
            }
            NodeReprs.Remove(cellObjectRepresentation);
            if (reprs.Count < 5) {
                reprs.Add(cellObjectRepresentation);
                cellObjectRepresentation.MakeInvisible();
                return;
            }
            Object.Destroy(cellObjectRepresentation.gameObject);
            
        }

        public void MoveOnOfferLayer(GameObject gameObject) {
            gameObject.layer = LayerMask.NameToLayer("ObjectInOffer");
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                Transform child = gameObject.transform.GetChild(i);
                MoveOnOfferLayer(child.gameObject);
            }
        }

        public void MoveOnDefaultLayer(GameObject gameObject) {
            gameObject.layer = LayerMask.NameToLayer("Default");
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                Transform child = gameObject.transform.GetChild(i);
                MoveOnDefaultLayer(child.gameObject);
            }
        }
    }
}