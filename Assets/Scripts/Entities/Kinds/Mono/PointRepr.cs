using System;
using Data;
using UnityEngine;

namespace Entities.Kinds.Mono
{
    public class PointRepr : EntityRepr<PointEntity>
    {
        public GameObject LeftArm;
        public GameObject RightArm;
        public GameObject LeftLeg;
        public GameObject RightLeg;
        public GameObject Legs;
        public GameObject Head;

        private Vector3 _leftLegLocal;
        private Vector3 _rightLegLocal;

        private float _legSpeed = 30f;
        
        private float legPos;
        public override void Init(PointEntity original) {
            base.Init(original);
            transform.parent = original.Parent.CellPivot;
            SetPos(new Vector3(original.Position.x, original.Position.y, -0.25f), original.Parent.CellPivot);
        }

        private void Awake() {
            _leftLegLocal = LeftLeg.transform.localPosition;
            _rightLegLocal = RightLeg.transform.localPosition;
        }

        private void Update()
        {
            legPos += Parent.DeltaPos;
            legPos %= Mathf.PI * 3;
            
            MoveLeg(LeftLeg, _leftLegLocal, legPos);
            MoveLeg(RightLeg, _rightLegLocal, (legPos + 1.5f * Mathf.PI) % (Mathf.PI * 3));
            
            Legs.transform.rotation = RotationHelper.RotateQ(Legs.transform.eulerAngles[2], Parent.PathAngle, _legSpeed * Time.deltaTime);
            Head.transform.rotation = 
                Quaternion.Euler(0, 0, 
                    RotationHelper.Limit(
                        RotationHelper.RotateF(
                            Head.transform.eulerAngles[2],
                            RotationHelper.AngleTo(
                                Parent.Parent.CellPivot.InverseTransformPoint(Head.transform.position),
                                Parent.Target
                                ), 
                            90 * Time.deltaTime
                            ),  
                        Parent.Angle, 
                        80)
                    );
            LeftArm.transform.rotation =
                RotationHelper.RotateQ(
                    LeftArm.transform.eulerAngles[2],
                    RotationHelper.AngleTo(
                        Parent.Parent.CellPivot.InverseTransformPoint(LeftArm.transform.position),
                        Parent.Target
                    ),
                    60 * Time.deltaTime
                );
            RightArm.transform.rotation =
                RotationHelper.RotateQ(
                    RightArm.transform.eulerAngles[2],
                    RotationHelper.AngleTo(
                        Parent.Parent.CellPivot.InverseTransformPoint(RightArm.transform.position),
                        Parent.Target
                    ),
                    60 * Time.deltaTime
                );
        }

        private void MoveLeg(GameObject leg, Vector3 legLocal, float t) {
            var y = 0 <= t && t<= Math.PI ? Mathf.Cos(t) : Mathf.Cos(t/2f + 1f/2f * Mathf.PI);
            var offset = new Vector3(0, y * 0.2f, 0);

            leg.transform.localPosition = legLocal + offset;
        }

        public void Rotate(float value) {
            transform.rotation = Quaternion.Euler(0, 0, value);
        }
    }
}