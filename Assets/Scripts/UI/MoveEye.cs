using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MoveEye : MonoBehaviour
    {
        [SerializeField] private Image eyelid1;
        [SerializeField] private Image eyelid2;

        public void SetEyelidCloseState(float state) {
            eyelid1.fillAmount = state;
            eyelid2.fillAmount = state;
        }
    }
}