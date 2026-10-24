using System;
using UnityEngine;

namespace Core.Asset
{
    [Serializable]
    public class RenderAssets
    {
        public Material ItemDropMaterial;
        public Material HealthBarMaterial;
        public Material FragmentMaterial;
        public Mesh ItemDropMesh;
        public Mesh HealthBarMesh;
        public Texture FragmentTexture;
        public Mesh FragmentMesh;
        public Material DebugLineMaterial;
        public Mesh ShadowRectMesh;
        public Material ShadowRectMaterial;
        public Texture DropAtlas;
        public Texture OreAtlas;
        public Mesh OreMesh;
        public Material OreMaterial;
    }
}