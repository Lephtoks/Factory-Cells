using System.Collections.Generic;
using Cells;
using Cells.Object;
using Core;
using Core.Locals;
using Data;
using DG.Tweening;
using UI.Cards;
using UnityEngine;

public class MainController : Singleton<MainController>, IUpdatable
{
    private readonly CellBehaviourArguments _cellBehaviourArguments = new();
    private bool _canTouchCell;
    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            _canTouchCell = !GameLocalBootstrap.Instance.canvasCardHolder.IsCardHovered();
        }

        Vector3 mousePosition = Input.mousePosition;
        _cellBehaviourArguments.WorldPos = GameStorage.Instance.Cam.ScreenToWorldPoint(mousePosition);
        Cell selectedCell = null;
        Vector3Int cellPos = Vector3Int.zero;
        
        foreach (var cell in GameStorage.Instance.GetCells()) {
            var tm = cell.tilemap;
            cellPos = tm.WorldToCell(_cellBehaviourArguments.WorldPos);
            if (tm.HasTile(cellPos))
            {
                selectedCell = cell;
                break;
            }
        }
        
        
        GameStorage.Instance.InfoCloud.gameObject.SetActive(false);
        GameStorage.Instance.InfoCloud.ResetIcons();

        if (selectedCell) {
            if (GameStorage.Instance.ActiveCard && selectedCell.Behaviour == CellBehaviours.TABLE && !Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0)) {
                IBlockRepr instanceNodeRepr = GameStorage.Instance.NodeReprs[0];
                instanceNodeRepr.MakePhantom();
                instanceNodeRepr.SetPos(new Vector3Int(cellPos.x, cellPos.y, -1), selectedCell.CellPivot);
            }
            if (_canTouchCell && Input.GetMouseButtonDown(0)) {
                _cellBehaviourArguments.MouseBeginPos = _cellBehaviourArguments.WorldPos;
                _cellBehaviourArguments.LocalMouseBeginPos = selectedCell.tilemap.WorldToLocal(_cellBehaviourArguments.MouseBeginPos); 
                _cellBehaviourArguments.Cell = selectedCell;
                selectedCell.OnClickBegin(_cellBehaviourArguments);
            }
            
            if (selectedCell.TryGetObject((Vector2Int)cellPos, out Block cellObject) && cellObject is IInventory inventory) {
                GameStorage.Instance.InfoCloud.transform.position = mousePosition;
                GameStorage.Instance.InfoCloud.gameObject.SetActive(true);
                foreach (var itemStack in inventory.GetItems()) {
                    GameStorage.Instance.InfoCloud.TryAddIcon(itemStack);
                }
            }

        }
        else {
            if (GameStorage.Instance.ActiveCard) {
                GameStorage.Instance.NodeReprs[0].MakeInvisible();
            }
            
        }

        if (_canTouchCell && _cellBehaviourArguments.Cell) {
            if (Input.GetMouseButtonUp(0) && (_cellBehaviourArguments.Cell.Behaviour == CellBehaviours.TABLE || (_cellBehaviourArguments.Cell.Behaviour == CellBehaviours.INVENTORY && selectedCell))) {
                _cellBehaviourArguments.Cell.OnClickRelease(_cellBehaviourArguments);
            }

            if (Input.GetMouseButton(0)) {
                _cellBehaviourArguments.Cell.OnClickMove(_cellBehaviourArguments);
            }

            if (Input.GetMouseButtonUp(0)) {
                _cellBehaviourArguments.Cell = null;
            }
        }
    }
    
    private readonly List<Card> _allOfferedCards = new List<Card>();

    public void ShowOffer(params BlockType[] types) {
        GameLocalBootstrap.Instance.shopScreen.gameObject.SetActive(true);
        for (int i = 0; i < types.Length; i++) {
            BlockType type = types[i];
            _allOfferedCards.Add(type.InstantiateInShop(index: i, total: types.Length));
        }
    }

    public void CloseOffer() {
        foreach (var card in _allOfferedCards) {
            if (card.GetBehaviour() == CardBehaviours.SHOP) {
                card.transform.DOKill();
                Object.Destroy(card.gameObject);
            }
        }
        GameLocalBootstrap.Instance.shopScreen.gameObject.SetActive(false);
        _allOfferedCards.Clear();
    }
}
