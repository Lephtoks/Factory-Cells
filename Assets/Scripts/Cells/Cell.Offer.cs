using System;
using Data;
using Data.GameManagement;
using Data.Offers;
using DG.Tweening;
using UI.Shop;
using UnityEngine;

namespace Cells
{
    public partial class Cell : IOfferable
    {
        internal Action UpdateShopPosition;
        
        public void DestroyInOffer() {
            transform.DOKill();
            Glow.Hide();
            Destroy(gameObject);
        }

        public void SelectedInOffer() {
            GameStorage.Instance.AddCell(this);
            Glow.Hide();
            SetBehaviour(CellBehaviours.INVENTORY);
            GameEvents.InvokeCellPositionUpdate();
        }

        public void AddToOffer(int row, int col, int totalRows, int totalCols) {
            UpdateShopPosition = () => ShopUpdater(row, col, totalRows, totalCols);
            Glow.Show();
            SetBehaviour(CellBehaviours.SHOP);
        }
        public void ShopUpdater(int row, int col, int totalRows, int totalCols)
        {
            BoundsInt bounds = tilemap.cellBounds;
            Camera cam = GameStorage.Instance.Cam;

            float cellWidth = bounds.size.x * tilemap.layoutGrid.cellSize.x;
            float cellHeight = bounds.size.y * tilemap.layoutGrid.cellSize.y;

            float worldHeight = cam.orthographicSize * 2f;
            float worldWidth = worldHeight * cam.aspect;

            float availableWidth = worldWidth * 0.6f;
            float availableHeight = worldHeight * 0.6f;

            float stepX = totalCols <= 1
                ? 0
                : availableWidth / (totalCols - 1);

            float stepY = totalRows <= 1
                ? 0
                : availableHeight / (totalRows - 1);

            float scaleByWidth = totalCols <= 1
                ? availableWidth / cellWidth
                : stepX / cellWidth;

            float scaleByHeight = totalRows <= 1
                ? availableHeight / cellHeight
                : stepY / cellHeight;

            float scale = Mathf.Min(scaleByWidth, scaleByHeight) * 0.9f;

            float vx = totalCols <= 1
                ? 0.5f
                : Mathf.Lerp(0.2f, 0.8f, (float)col / (totalCols - 1));

            float vy = totalRows <= 1
                ? 0.5f
                : Mathf.Lerp(0.8f, 0.2f, (float)row / (totalRows - 1));

            Vector3 worldPos = cam.ViewportToWorldPoint(new Vector3(vx, vy, 15f));

            transform.DOKill();

            transform
                .DOScale(_baseScale * scale, 0.3f)
                .SetEase(Ease.OutCubic);

            transform
                .DOMove(worldPos, 0.3f)
                .SetEase(Ease.OutCubic);
        }

        internal void OnScreenSizeChanged(int width, int height)
        {
            UpdateShopPosition();
        }

        internal void OnCameraUpdate()
        {
            UpdateShopPosition();
        }
    }
}