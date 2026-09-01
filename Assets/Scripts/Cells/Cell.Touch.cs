using Cells.Object;
using Data.GameManagement;
using Interactions;
using UnityEngine;

namespace Cells
{
    public partial class Cell : ITouchable
    {
        public float GetDepth() {
            return DepthLayers.CELLS;
        }

        public bool CapturesClick() {
            return _behaviour == CellBehaviours.TABLE;
        }

        public bool IsSelected(Vector3 mousePos, Vector3 worldPos) {
            var cellPos = tilemap.WorldToCell(worldPos);
            return tilemap.HasTile(cellPos);
        }

        public void Select(Vector3 mousePos, Vector3 worldPos, int capturedButton, bool captured) {
            MainController.Instance.CellBehaviourArguments.CapturedButton = capturedButton;
            var cellPos = tilemap.WorldToCell(worldPos);
            
            if (GameStorage.Instance.BuildOption.GetActiveBlock() != null && _behaviour == CellBehaviours.TABLE &&
                ( capturedButton == -1 || 
                  (!Input.GetMouseButton(MainController.Instance.CellBehaviourArguments.CapturedButton) &&
                   !Input.GetMouseButtonUp(MainController.Instance.CellBehaviourArguments.CapturedButton)))) {
                BlockRepr instanceNodeRepr = GameStorage.Instance.NodeReprs[0];
                instanceNodeRepr.MakePhantom();
                instanceNodeRepr.SetPos(new Vector3Int(cellPos.x, cellPos.y, -1), CellPivot);
            }

            if (capturedButton != -1) {
                if (Input.GetMouseButtonDown(MainController.Instance.CellBehaviourArguments.CapturedButton)) {
                    MainController.Instance.CellBehaviourArguments.MouseBeginPos = worldPos;
                    MainController.Instance.CellBehaviourArguments.ObjectCaptured = captured;
                    MainController.Instance.CellBehaviourArguments.LocalMouseBeginPos = tilemap.WorldToLocal(worldPos);
                    OnClickBegin(MainController.Instance.CellBehaviourArguments);
                }

                if (Input.GetMouseButtonUp(MainController.Instance.CellBehaviourArguments.CapturedButton)) {
                    OnClickRelease(MainController.Instance.CellBehaviourArguments);
                }

                if (Input.GetMouseButton(MainController.Instance.CellBehaviourArguments.CapturedButton)) {
                    OnClickMove(MainController.Instance.CellBehaviourArguments);
                }
            }

            if (TryGetObject((Vector2Int)cellPos, out Block cellObject) && cellObject is IInventory inventory) {
                GameStorage.Instance.InfoCloud.transform.position = mousePos;
                GameStorage.Instance.InfoCloud.gameObject.SetActive(true);
                foreach (var itemStack in inventory.GetItems()) {
                    GameStorage.Instance.InfoCloud.TryAddIcon(itemStack);
                }
            }
            else {
                GameStorage.Instance.InfoCloud.gameObject.SetActive(false);
            }
        }
    }
}