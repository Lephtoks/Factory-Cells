using Cells;
using Data.GameManagement;
using UnityEngine;

namespace UI
{
    public class SelectionZone : MonoBehaviour
    {
        public RectTransform RectTransform;
        public Cell Cell { get; private set; }
        public Vector2Int StartPos { get; private set; }
        public Vector2Int EndPos { get; private set; }

        public void SetStartPos(Cell cell, Vector2Int startPos) {
            Cell = cell;
            StartPos = startPos;
        }
        public void Extend(Vector2Int endPos) {
            EndPos = endPos;
            UpdateDisplay();
        }

        public void UpdateDisplay() {
            if (!Cell) return;
            
            var cam = GameStorage.Instance.Cam;

            var sp = cam.WorldToScreenPoint(Cell.tilemap.GetCellCenterWorld((Vector3Int)StartPos));
            var ep = cam.WorldToScreenPoint(Cell.tilemap.GetCellCenterWorld((Vector3Int)EndPos));

            RectTransform.localPosition = sp;
            RectTransform.sizeDelta = new Vector2(ep.x - sp.x, ep.y - sp.y);
        }
    }
}