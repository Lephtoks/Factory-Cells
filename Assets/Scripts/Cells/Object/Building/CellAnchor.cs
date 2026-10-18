using System.Collections.Generic;
using Cells.Object.Building.Mono;
using Core;
using Data;
using Data.GameManagement;
using Economics;
using Entities;
using GameRenderer;
using UnityEngine;

namespace Cells.Object.Building
{
    public class CellAnchor: Block, IRepresentable<DefaultRepr>, IKillable, IShadow
    {
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public bool Dead { get; set; }
        public override BlockType BlockType => BlockTypes.CELL_ANCHOR;
        public DefaultRepr LivingRepresentation { get; set; }
        
        public CellAnchor(Cell parent, Vector2Int pos) : base(parent, pos) {
            MaxHealth = 1000;
            Health = 1000;
        }

        public override void UpdateMove() {
            base.UpdateMove();
        }

        public override void WhenBeingAddedToCell() {
            base.WhenBeingAddedToCell();
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

        public static Block Create(Cell parent, BlockRepr repr) {
            return ((IRepresentable<DefaultRepr>) new CellAnchor(parent, new Vector2Int((int) repr.transform.localPosition.x, (int) repr.transform.localPosition.y))).AssignRepresentation(repr);
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