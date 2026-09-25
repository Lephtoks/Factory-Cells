using System;
using System.Linq;
using Core;
using Data;
using UnityEngine;

namespace Cells.Object.Building.Mono
{
    public class ConveyorRepr : BlockRepr<Conveyor>
    {
        public Conveyor OriginalConveyor;
        private SpriteRenderer _spriteRenderer;
        private Sprite[] _animationSprites;
        private Vector3 _originalScale;

        public DirectionFlag Connections;
        [NonSerialized] public Direction Direction;

        private void Awake() {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalScale = transform.localScale;
        }

        public override void UseSettings(RepresentationSettings representationSettings) {
            base.UseSettings(representationSettings);
            UpdateConveyorDisplay(representationSettings.Direction);
            Connections = new DirectionFlag();
            foreach (var dir in (Direction[])Enum.GetValues(typeof(Direction))) {
                if (Cell && Cell.TryGetObject(new Vector2Int((int) transform.localPosition.x, (int) transform.localPosition.y) + dir.ToVector2Int(), out Block block)) {
                    if (dir == Direction) {
                        Connections += Direction;
                    } else if (block is IInventoryOut inventoryOut && inventoryOut.OutDirections().Contains(dir.Opposite())) {
                        Connections += dir;
                    }
                };
            }
        }

        public override void Init(Conveyor original) {
            MakeReal();
            OriginalConveyor = original;
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -0.25f) + new Vector3(0.5f, 0.5f, 0);
            transform.localRotation = Direction.EAST.ToQuaternion();
        }

        public void UpdateConveyorDisplay(Direction conveyorDirection) {
            Direction = conveyorDirection;
            _animationSprites = AssetProvider.Instance.GetConveyorAnimationList(conveyorDirection, Connections);
            if (conveyorDirection == Direction.WEST && (Connections.ToByte() == 8 || Connections.ToByte() == 0 || Connections.ToByte() == 10 ||  Connections.ToByte() == 2)) {
                transform.localScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
            } else if (conveyorDirection == Direction.SOUTH && (Connections.ToByte() == 4 || Connections.ToByte() == 5)) {
                transform.localScale = new Vector3(_originalScale.x, -_originalScale.y, _originalScale.z);
            }
            else {
                transform.localScale = _originalScale;
            }
        }

        public void Update() {
            if (!Cell) return;
            if (_animationSprites.Length == 0) return;
            _spriteRenderer.sprite = _animationSprites[Mathf.RoundToInt((Cell.SynchronousConveyorTime % 1) * (_animationSprites.Length-1))];
        }
    }
}