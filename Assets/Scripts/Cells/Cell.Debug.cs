using GameDebug;

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
    }
}