using System;
using Core;
using Data;
using UnityEngine;

namespace Cells.Object.Building.Mono
{
    public class ConveyorRepr : BlockRepr<Conveyor>
    {
        private Conveyor _originalConveyor;
        private SpriteRenderer _spriteRenderer;
        private Sprite[] _animationSprites;
        private Vector3 _originalScale;

        public DirectionFlag Connections; 

        private void Awake() {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalScale = transform.localScale;
        }

        public override void UseSettings(RepresentationSettings representationSettings) {
            base.UseSettings(representationSettings);
            transform.localRotation = representationSettings.Direction.ToQuaternion();

        }

        public override void Init(Conveyor original) {
            MakeReal();
            _originalConveyor = original;
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -0.25f) + new Vector3(0.5f, 0.5f, 0);
            transform.localRotation = Direction.EAST.ToQuaternion();
        }

        public void UpdateConveyorDisplay() {
            if (_originalConveyor == null) return;
            _animationSprites = AssetProvider.Instance.GetConveyorAnimationList(_originalConveyor, _originalConveyor.LivingRepresentation.Connections);
            if (_originalConveyor.Direction == Direction.WEST) {
                transform.localScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
            } else if (_originalConveyor.Direction == Direction.SOUTH && Connections.Contains(Direction.SOUTH)) {
                transform.localScale = new Vector3(_originalScale.x, -_originalScale.y, _originalScale.z);
            }
        }

        public void Update() {
            if (_originalConveyor == null) return;
            if (_animationSprites.Length == 0) return;
            _spriteRenderer.sprite = _animationSprites[Mathf.RoundToInt((_originalConveyor.Parent.SynchronousConveyorTime % 1) * (AssetProvider.Instance.registry.ConveyorRightSprites.Length-1))];
        }
    }
}