using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace GameDebug
{
    public class DebugManager : Singleton<DebugManager>
    {
        private InputSystem_Actions actions;
        
        public static bool HITBOXES_VISIBLE;
        
        public override void Init() {
            base.Init();
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
            actions = new InputSystem_Actions();
            actions.Debug.Enable();
            HITBOXES_VISIBLE = false;
            actions.Debug.Showhitboxes.performed += ToggleHitboxes;
        }

        public override void Dispose() {
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
            actions.Debug.Showhitboxes.performed -= ToggleHitboxes;
        }

        private void ToggleHitboxes(InputAction.CallbackContext obj) {
            HITBOXES_VISIBLE = !HITBOXES_VISIBLE;
            Debug.Log(HITBOXES_VISIBLE);
        }

        private void OnEndCameraRendering(
            ScriptableRenderContext context,
            Camera camera)
        {
            DebugDrawer.OnPostRender(camera);
        }
    }
}