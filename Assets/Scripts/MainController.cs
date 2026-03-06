using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Bulding;
using Cells.Object.Bulding.Mono;
using Data;
using Global;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainController : MonoBehaviour
{
    void Update()
    {
        Vector3 worldPos = GameStorage.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
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

        if (selectedCell != null) {
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
}
