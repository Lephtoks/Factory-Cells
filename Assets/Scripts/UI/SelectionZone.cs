using Cells;
using Data.GameManagement;
using UnityEngine;

namespace UI
{
    public class SelectionZone : MonoBehaviour
    {
        public RectTransform RectTransform;
        public Canvas Canvas;               // assign the parent Canvas
        public Cell Cell { get; private set; }
        public Vector2Int StartPos { get; private set; }
        public Vector2Int EndPos { get; private set; }

        public void SetStartPos(Cell cell, Vector2Int startPos)
        {
            Cell = cell;
            StartPos = startPos;
        }

        public void Extend(Vector2Int endPos)
        {
            EndPos = endPos;
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            if (Cell == null || Canvas == null) return;

            Vector2Int min = Vector2Int.Min(StartPos, EndPos);
            Vector2Int max = Vector2Int.Max(StartPos, EndPos);

            Vector3 worldMin = Cell.tilemap.CellToWorld((Vector3Int)min);
            Vector3 worldMax = Cell.tilemap.CellToWorld((Vector3Int)(max) + Vector3Int.one);

            Camera cam = GameStorage.Instance.Cam;

            Vector2 screenMin = RectTransformUtility.WorldToScreenPoint(cam, worldMin);
            Vector2 screenMax = RectTransformUtility.WorldToScreenPoint(cam, worldMax);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Canvas.transform as RectTransform,
                screenMin,
                Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
                out Vector2 localMin);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Canvas.transform as RectTransform,
                screenMax,
                Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
                out Vector2 localMax);

            RectTransform.anchoredPosition = localMin;
            RectTransform.sizeDelta = localMax - localMin;
        }
    }
}