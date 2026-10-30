using System;
using System.Collections.Generic;
using GameRenderer;
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
        public Texture BlockAtlas;
        public Material BlockMaterial;
        public Mesh BlockMesh;
    }
}