using Cells.Object;
using Core;
using UnityEngine;

namespace GameRenderer
{
    public class OreRenderer : ObjectIndirectRenderer<Vector2Int, OreInstance>
    {
        public OreRenderer(GameObject gameObject, Mesh quad, Material material, int capacity = 10000) : base(
            gameObject, quad, material, capacity) {
        }

        public override void Init() {
            base.Init();
            Properties.SetTexture("_SpriteAtlas", AssetProvider.Instance.registry.render.OreAtlas);
        }
    }
}