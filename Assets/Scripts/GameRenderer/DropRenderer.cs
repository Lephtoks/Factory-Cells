using System;
using System.Collections.Generic;
using System.Linq;
using Cells.Object;
using Core;
using UnityEngine;

namespace GameRenderer
{
    public class DropRenderer : ObjectIndirectRenderer<Vector2Int, DroppedItem>
    {
        public DropRenderer(GameObject gameObject, Mesh quad, Material material, int capacity = 10000) : base(
            gameObject, quad, material, capacity) {
        }
        public float DropPositionTime;
        public override void Render() {
            UpdateCPU();
            base.Render();
        }
        private void UpdateCPU()
        {
            Properties.SetFloat("_GlobalTime", DropPositionTime);
        }


        public override void Init() {
            base.Init();
            Properties.SetTexture("_itemSprite", AssetProvider.Instance.registry.render.FragmentTexture);
        }
    }
}