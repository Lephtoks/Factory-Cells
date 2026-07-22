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
            Destroy(gameObject);
        }

        public void SelectedInOffer() {
            GameStorage.Instance.AddCell(this);
            SetBehaviour(CellBehaviours.INVENTORY);
            GameEvents.InvokeCellPositionUpdate();
        }

        public void AddToOffer(int row, int col, int totalRows, int totalCols) {
            UpdateShopPosition = () => ShopUpdater(row, col, totalRows, totalCols);
            SetBehaviour(CellBehaviours.SHOP);
        }
        public void ShopUpdater(int row, int col, int totalRows, int totalCols)
        {
            float parentWidth = ShopScreen.Instance.rectTransform.rect.width;
            float parentHeight = ShopScreen.Instance.rectTransform.rect.height;
            
            BoundsInt bounds = tilemap.cellBounds;
            var cam = GameStorage.Instance.Cam;
            float cardWidth = bounds.size.x * tilemap.layoutGrid.cellSize.x * Screen.width / (cam.orthographicSize * 2f * cam.aspect);
            float cardHeight = bounds.size.y * tilemap.layoutGrid.cellSize.y * Screen.height / (cam.orthographicSize * 2f);
            
            float minX = -parentWidth * 0.3f;
            float maxX = parentWidth * 0.3f;
            
            float minY = -parentHeight * 0.3f;
            float maxY = parentHeight * 0.3f;
            
            float availableWidth = maxX - minX;
            float availableHeight = maxY - minY;
            
            
            
            float scaleByHeight = availableHeight / cardHeight;
            
            
            
            float scaleByWidth;
            
            if (totalCols <= 1)
            {
                scaleByWidth = availableWidth / cardWidth;
            }
            else
            {
                float step = availableWidth / (totalCols - 1);
                scaleByWidth = step / cardWidth;
            }
            
            float scale = Mathf.Min(scaleByWidth, scaleByHeight) * 0.9f;
            
            
            float x;
            
            if (totalCols <= 1)
            {
                x = 0f;
            }
            else
            {
                float step = availableWidth / (totalCols - 1);
                x = minX + step * col;
            }
            
            float y = 0f;
            
            transform.DOKill();
            transform
                .DOScale(_baseScale * scale, 0.3f)
                .SetEase(Ease.OutCubic);
            Vector3 screenPos = new Vector3(
                Screen.width * 0.5f + x,
                Screen.height * 0.5f + y,
                15
            );
            transform
                .DOMove(cam.ScreenToWorldPoint(screenPos), 0.3f)
                .SetEase(Ease.OutCubic);
        }

        internal void OnScreenSizeChanged(int width, int height)
        {
            UpdateShopPosition();
        }
    }
}