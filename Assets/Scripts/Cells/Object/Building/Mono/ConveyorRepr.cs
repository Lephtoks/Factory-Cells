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

        private void Awake() {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public override void Init(Conveyor original) {
            MakeReal();
            _originalConveyor = original;
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -0.25f) + new Vector3(0.5f, 0.5f, 0);
            var vector2Int = original.Direction.ToVector2Int();
            transform.localRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(vector2Int.x, vector2Int.y, 0));
        }

        public void Update() {
            if (_originalConveyor == null) return;
            _spriteRenderer.sprite = AssetProvider.Instance.registry.ConveyorRightSprites[Mathf.RoundToInt(_originalConveyor.Parent.SynchronousConveyorTime * (AssetProvider.Instance.registry.ConveyorRightSprites.Length-1))];
        }
    }
}