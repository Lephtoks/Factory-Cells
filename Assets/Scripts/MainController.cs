using Cells.Object;
using Core;
using Core.Locals;
using Data;
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

    public void ShowOffer(params CellObjectType[] types) {
        for (int i = 0; i < types.Length; i++) {
            CellObjectType type = types[i];
            var card = type.Instantiate();
            card.transform.SetParent(GameLocalBootstrap.Instance.shopScreen.transform);
            card.enabled = false;
            var cardInShop = card.gameObject.AddComponent<CardInShop>();
            cardInShop.index = i;
            cardInShop.total = types.Length;
        }
    }

    public override void Init() {
        base.Init();
        ShowOffer(
            CellObjectTypes.DRILL,
            CellObjectTypes.DRILL,
            CellObjectTypes.DRILL
            );
    }
}
