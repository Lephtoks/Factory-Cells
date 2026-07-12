using UnityEngine;

namespace Data.GameManagement
{
    public partial class GameStorage
    {
        public void MoveOnOfferLayer(GameObject gameObject) {
            gameObject.layer = LayerMask.NameToLayer("ObjectInOffer");
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                Transform child = gameObject.transform.GetChild(i);
                MoveOnOfferLayer(child.gameObject);
            }
        }

        public void MoveOnDefaultLayer(GameObject gameObject) {
            gameObject.layer = LayerMask.NameToLayer("Default");
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                Transform child = gameObject.transform.GetChild(i);
                MoveOnDefaultLayer(child.gameObject);
            }
        }
    }
}