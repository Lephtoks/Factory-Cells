using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MoveEye : MonoBehaviour
    {
        [NonSerialized] public bool IsClosing;
        [SerializeField] private Image eyelid1;
        [SerializeField] private Image eyelid2;

        public void SetEyelidCloseState(float state) {
            eyelid1.fillAmount = state;
            eyelid2.fillAmount = state;
        }

        public void Close() {
            eyelid1.DOFillAmount(1, 0.3f).SetEase(Ease.OutBack);
            eyelid2.DOFillAmount(1, 0.3f).SetEase(Ease.OutBack);
            IsClosing = true;
        }

        public void Open() {
            eyelid1.DOFillAmount(0, 0.3f).SetEase(Ease.InBack);
            eyelid2.DOFillAmount(0, 0.3f).SetEase(Ease.InBack);
            IsClosing = false;
        }
    }
}