using System.Collections.Generic;
using Cells.Object;
using Core;
using Core.Locals;
using Data;
using DG.Tweening;
using UI.Cards;
using UnityEngine;

public class MainController : Singleton<MainController>, IUpdatable
{
    public void Update()
    {
        Vector3 worldPos = GameStorage.Instance.Cam.ScreenToWorldPoint(Input.mousePosition);
        Cell selectedCell = null;
        Vector3Int cellPos = Vector3Int.zero;
        
        // перебираем тайлмапы в порядке приоритета
        foreach (var cell in GameStorage.Instance.GetCells()) {
            var tm = cell.tilemap;
            cellPos = tm.WorldToCell(worldPos);
            if (tm.HasTile(cellPos))
            {
                selectedCell = cell;
                break;
            }
        }
        
        
        GameStorage.Instance.InfoCloud.gameObject.SetActive(false);
        GameStorage.Instance.InfoCloud.ResetIcons();

        if (selectedCell) {
            if (selectedCell.TryGetObject((Vector2Int)cellPos, out CellObject cellObject)) {
                GameStorage.Instance.InfoCloud.transform.position = Input.mousePosition;
                GameStorage.Instance.InfoCloud.gameObject.SetActive(true);
                foreach (var itemStack in cellObject.GetItems()) {
                    GameStorage.Instance.InfoCloud.TryAddIcon(itemStack);
                }
            };
            if (Input.GetMouseButtonUp(0))
            {
                selectedCell.behaviour.OnClick(worldPos, cellPos, selectedCell);
            }
            
        }
    }
    
    private readonly List<Card> _allOfferedCards = new List<Card>();

    public void ShowOffer(params CellObjectType[] types) {
        GameLocalBootstrap.Instance.shopScreen.gameObject.SetActive(true);
        for (int i = 0; i < types.Length; i++) {
            CellObjectType type = types[i];
            _allOfferedCards.Add(type.InstantiateInShop(type, index: i, total: types.Length));
        }
    }

    public void CloseOffer() {
        foreach (var card in _allOfferedCards) {
            if (card.GetBehaviour() == CardBehaviours.SHOP) {
                card.transform.DOKill();
                GameObject.Destroy(card.gameObject);
            }
        }
        GameLocalBootstrap.Instance.shopScreen.gameObject.SetActive(false);
        _allOfferedCards.Clear();
    }
}
