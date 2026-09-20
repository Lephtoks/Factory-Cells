using Data.GameManagement;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class DayClock : MonoBehaviour
    {
        public RectTransform Pointer;
        public RectTransform thisRect;

        public void UpdatePointer() {
            Pointer.DOAnchorPos(new Vector2(GameStorage.Instance.Time.DayTime * thisRect.sizeDelta.x / GameStorage.Instance.Time.HoursInDay, 0), 0.5f).SetEase(Ease.OutBounce);
        }
    }
}