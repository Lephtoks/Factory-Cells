using Data;
using UnityEngine;

namespace Cells.Object.Building.Mono
{
    public class WindGenRepr : BlockRepr<WindGen>
    {
        
        public override void UseSettings(RepresentationSettings representationSettings) {
            base.UseSettings(representationSettings);
            transform.localRotation = representationSettings.Direction.ToQuaternion();

        }

        public override void Init(WindGen original) {
            MakeReal();
            transform.parent = original.Parent.CellPivot;
            transform.localPosition = new Vector3(original.Position.x, original.Position.y, -0.25f) + new Vector3(0.5f, 0.5f, 0);
            transform.localRotation = original.Direction.ToQuaternion();
        }
    }
}