using System;
using System.Collections.Generic;
using Cells.Object;
using Cells.Object.Node;
using Core;
using Data.GameManagement;
using DG.Tweening;
using Entities;
using UnityEngine;

namespace Cells
{
    public partial class Cell
    {
        private readonly List<Bullet> _bullets = new();
        
        public void AddBullet(Bullet bullet) {
            _bullets.Add(bullet);
            _colliders.Add(bullet);
        }

        public void RemoveBullet(Bullet bullet) {
            _bullets.Remove(bullet);
            _colliders.Remove(bullet);
        }

        public List<Bullet> GetBullets() => _bullets;

        public void UpdateBullets() {
            foreach (var bullet in _bullets) {
                bullet.Update();
            }
        }

        public void DrawBullets() {
            var rp = new RenderParams(AssetProvider.Instance.registry.render.ItemDropMaterial) {
                // layer = gameObject.layer
            };
        
            foreach (var bullet in GetBullets()) {
                var sprite = bullet.BulletType.bulletSprite;
                if (sprite == null) continue;
        
                _droppedItemBlock.SetTexture("_MainTex", sprite.texture);
                rp.matProps = _droppedItemBlock;
        
                var worldPos = CellPivot.TransformPoint(bullet.Position);
                var matrix = Matrix4x4.TRS(worldPos, CellPivot.rotation, CellPivot.lossyScale * 0.25f);
        
                Graphics.RenderMesh(rp, AssetProvider.Instance.registry.render.ItemDropMesh, 0, matrix);
            }
        }
    }
}