using GameDebug;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        public void DrawHitboxes() {
            if (!DebugManager.HITBOXES_VISIBLE) return;

            foreach (var collider in _colliders) {
                collider.Hitbox.Draw();
            }
        }

        private void DrawNavigation() {
            if (!DebugManager.NAVIGATION_VISIBLE) return;

            foreach (var node in NavTree.Nodes) {
                DebugDrawer.Circle(tilemap.LocalToWorld(node.Position), 0.1f,
                    Color.yellow, 8);
            }
        }
    }
}