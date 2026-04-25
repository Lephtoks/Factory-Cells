using DG.Tweening;
using UnityEngine;

namespace Cells
{
    public class CellFloating : MonoBehaviour
    {
        [SerializeField] float moveDistance = 0.3f;
        [SerializeField] float moveDuration = 1.5f;

        [SerializeField] float rotateAngle = 5f;
        [SerializeField] float rotateDuration = 2f;

        void Start()
        {
            Vector3 startPos = transform.localPosition;

            float moveOffset = Random.Range(0.8f, 1.2f);
            float rotateOffset = Random.Range(0.8f, 1.2f);
            float durationOffset = Random.Range(0.85f, 1.15f);
            float startDelay = Random.Range(0f, 1.5f);

            float finalMove = moveDistance * moveOffset;
            float finalRotate = rotateAngle * rotateOffset;

            transform.localRotation = Quaternion.Euler(0, 0, -finalRotate);

            Sequence seq = DOTween.Sequence().SetDelay(startDelay)
                .SetLoops(-1, LoopType.Yoyo);

            seq.Join(
                transform.DOLocalMoveY(startPos.y + finalMove, moveDuration * durationOffset)
                    .SetEase(Ease.InOutSine)
            );

            seq.Join(
                transform.DOLocalRotate(new Vector3(0, 0, finalRotate), rotateDuration * durationOffset)
                    .SetEase(Ease.InOutSine)
            );
        }
    }
}