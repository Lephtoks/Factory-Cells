using System;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Cells.Object.Building.Mono
{
    public class TurretRepr : BlockRepr<Turret>
    {
        public Transform GunTransform;
        public Transform TowerTransform;
        public Transform BulletCreatorTransform;
        public Turret Original { get; private set; }

        public override void Init(Turret original) {
            MakeReal();
            transform.parent = original.Parent.CellPivot;
            Original = original;
        }

        public void Shot() {
            GunTransform.DOKill();
            DOTween.Sequence()
                .Append(GunTransform.DOLocalMoveY(0.1f, 0.1f))
                .Append(GunTransform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuad));
        }

        private void Update() {
            if (Original == null) return;
            
            if (Cell) Original.TargetPosition = Cell.NearestEntity(Original.Position + Vector2.one * 0.5f).Position;
            
            TowerTransform.localRotation = RotationHelper.RotateQ(
                TowerTransform.localRotation.eulerAngles.z,
                RotationHelper.AngleTo(Original.Position + Vector2.one * 0.5f, Original.TargetPosition),
                30 * Time.deltaTime
            );
        }
    }
}