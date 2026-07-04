using Core;
using Data;
using UnityEngine;

namespace Cells.Object.Building.Mono
{
    public class ConveyorRepr : BlockRepr<Conveyor>
    {
        Animator _animator;
        public override void Init(Conveyor original) {
            MakeReal();
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -0.25f) + new Vector3(0.5f, 0.5f, 0);
            var vector2Int = original.Direction.ToVector2Int();
            transform.localRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(vector2Int.x, vector2Int.y, 0));
            _animator = GetComponent<Animator>();
            UpdateAnimation();
        }

        public void UpdateAnimation() {
            var normal = (Time.time % (4f/3f)) / (4f/3f);
            _animator.Play("conveyor", 0, normal);
        }
    }
}