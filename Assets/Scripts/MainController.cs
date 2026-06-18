using System.Collections.Generic;
using Cells;
using Cells.Object;
using Core;
using Core.Locals;
using Data;
using DG.Tweening;
using Interactions;
using UI.Cards;
using UnityEngine;

public class MainController : Singleton<MainController>, IUpdatable
{
    public readonly InteractionManager InteractionManager = new();

    public readonly CellBehaviourArguments CellBehaviourArguments = new();
    private bool _canTouchCell;
    public void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        CellBehaviourArguments.WorldPos = GameStorage.Instance.Cam.ScreenToWorldPoint(mousePos);
        InteractionManager.Select(mousePos, CellBehaviourArguments.WorldPos);
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
