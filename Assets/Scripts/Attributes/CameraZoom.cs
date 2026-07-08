using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float zoomMin = 2f;
        [SerializeField] private float zoomMax = 20f;

        private Camera _cam;

        private void Awake() {
            _cam = GetComponent<Camera>();
        }

        private void Update() {
            float scroll = Input.mouseScrollDelta.y;
            if (scroll == 0)
                return;
            
            Vector3 before = _cam.ScreenToWorldPoint(Input.mousePosition);

            float camOrthographicSize = Mathf.Clamp(
                _cam.orthographicSize - scroll * zoomSpeed, zoomMin, zoomMax);
            _cam.DOOrthoSize(camOrthographicSize, 0.5f);

            var old = _cam.orthographicSize;
            _cam.orthographicSize = camOrthographicSize;
            Vector3 after = _cam.ScreenToWorldPoint(Input.mousePosition);
            _cam.orthographicSize = old;

            transform.DOMove(before - after + transform.position, 0.5f);
            
        }
    }
}