using System;
using Data;
using DG.Tweening;
using Entities;
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
            if (Cell) {
                Entity nearestEntity = Cell.NearestEntity(Original.Position + Vector2.one * 0.5f);
                if (nearestEntity != null) {
                    Original.TargetPosition = nearestEntity.Position;
                    float eulerAnglesZ = TowerTransform.localRotation.eulerAngles.z;
                    float targetAngle = RotationHelper.AngleTo(Original.Position + Vector2.one * 0.5f, Original.TargetPosition);

                    TowerTransform.localRotation = RotationHelper.RotateQ(
                        eulerAnglesZ,
                        targetAngle,
                        60 * Time.deltaTime
                    );
                    float deltaAngle = Mathf.DeltaAngle(eulerAnglesZ, targetAngle);
                    Original.Targeted = deltaAngle is <= 5 and >= -5;
                }
                else {
                    Original.Targeted = false;
                }
            }
        }
    }
}