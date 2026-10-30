using System.Collections.Generic;
using Data.GameManagement;
using GameRenderer;
using UnityEngine;

namespace Cells.Object.Building
{
    public class CellAnchor: Block, IKillable, IShadow
    {
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public override BlockType BlockType => BlockTypes.CELL_ANCHOR;
        
        public CellAnchor() : base() {
            MaxHealth = 1000;
            Health = 1000;
        }

        public override void UpdateMove() {
            base.UpdateMove();
        }

        public override void Init(Vector2Int pos) {
            base.Init(pos);
            Parent.Unlock(new CellAnchorLocker());
        }

        public override void WhenBeingAddedToCellLate() {
            base.WhenBeingAddedToCellLate();
            Parent.Lock(new ExporterLocker());
            GameStorage.Instance.BuildOption.EnqueueNecessary(BlockTypes.EXPORTER);
        }

        public override void OnDestroy() {
            Parent.InitiateCellDestruction();
        }

        public IEnumerable<ShadowRect> CreateShadow() {
            yield return new ShadowRect(Position, new Vector2(1, 0.5f), 0.25f);
            yield return new ShadowRect(Position, new Vector2(2/16f, 2/16f), 0.6f);
            yield return new ShadowRect(Position + Vector2.right * 14/16f, new Vector2(2/16f, 2/16f), 0.6f);
            yield return new ShadowRect(Position + new Vector2(3/16f, 6/16f), new Vector2(2/16f, 2/16f), 0.6f);
            yield return new ShadowRect(Position + new Vector2(11/16f, 6/16f), new Vector2(2/16f, 2/16f), 0.6f);
        }

        public int ShadowCount => 5;
    }
}