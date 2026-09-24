using Data.GameManagement;
using UnityEngine;

namespace Particles
{
    public class BGParticlesParent : MonoBehaviour
    {
        [SerializeField] private float referenceOrthoSize = 5f;
        
        private void LateUpdate() {
            var targetCamera = GameStorage.Instance.Cam;
            if (!targetCamera || !targetCamera.orthographic)
                return;

            float scaleFactor = targetCamera.orthographicSize / referenceOrthoSize;
            transform.localScale = Vector3.one * scaleFactor;
        }
    }
}