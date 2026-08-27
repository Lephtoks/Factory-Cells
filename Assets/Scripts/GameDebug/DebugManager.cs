using Cells;
using Core;
using Data.GameManagement;
using Entities;
using Entities.Kinds;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace GameDebug
{
    public class DebugManager : Singleton<DebugManager>
    {
        private InputSystem_Actions actions;
        
        public static bool HITBOXES_VISIBLE;
        
        public static bool NAVIGATION_VISIBLE;
        
        public override void Init() {
            base.Init();
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
            actions = new InputSystem_Actions();
            actions.Debug.Enable();
            HITBOXES_VISIBLE = false;
            NAVIGATION_VISIBLE = false;
            actions.Debug.Showhitboxes.performed += ToggleHitboxes;
            actions.Debug.Testaction.performed += DoTestAction;
            actions.Debug.Shownavigation.performed += ToggleNavigation;
        }

        public override void Dispose() {
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
            actions.Debug.Showhitboxes.performed -= ToggleHitboxes;
            actions.Debug.Testaction.performed -= DoTestAction;
            actions.Debug.Shownavigation.performed -= ToggleNavigation;
        }

        private void ToggleHitboxes(InputAction.CallbackContext obj) {
            HITBOXES_VISIBLE = !HITBOXES_VISIBLE;
            Debug.Log(HITBOXES_VISIBLE);
        }

        private void ToggleNavigation(InputAction.CallbackContext obj) {
            NAVIGATION_VISIBLE = !NAVIGATION_VISIBLE;
            Debug.Log(NAVIGATION_VISIBLE);
        }

        private void DoTestAction(InputAction.CallbackContext obj) {
            Cell table = GameStorage.Instance.CellInventory.GetTable();
            table.AddEntity(new PointEntity(table, new Vector2(4, 6)));
            Debug.Log("Entity added");
        }

        private void OnEndCameraRendering(
            ScriptableRenderContext context,
            Camera camera)
        {
            DebugDrawer.OnPostRender(camera);
        }
    }
}