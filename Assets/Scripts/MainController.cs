using System.Collections.Generic;
using Cells.Object.Bulding;
using Cells.Object.Bulding.Mono;
using Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 worldPos = GameStorage.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int clickedPos = Vector2Int.zero;
            Cell clickedCell = null;

            // перебираем тайлмапы в порядке приоритета
            foreach (var cell in GameStorage.Instance.GetCells()) {
                var tm = cell.tilemap;
                Vector3Int cellPos = tm.WorldToCell(worldPos);
                if (tm.HasTile(cellPos))
                {
                    cell.behaviour.OnClick(worldPos, cellPos, cell);
                    break;
                }
            }
        }
    }
}
