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
    }
}