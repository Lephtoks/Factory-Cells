using System.Collections.Generic;
using Cells.Object;
using Data.GameManagement;
using JetBrains.Annotations;

namespace Data
{
    public class BlockBuildOption
    {
        private readonly Queue<BlockType> _necessaryBlocks = new();

        public void EnqueueNecessary(BlockType block) {
            _necessaryBlocks.Enqueue(block);
            GameStorage.Instance.Representer.SetCurrentBlockRepr();
        }
        public void DequeueNecessary() {
            _necessaryBlocks.Dequeue();
        }

        [CanBeNull]
        public BlockType GetActiveBlock() {
            if (_necessaryBlocks.Count > 0) {
                return _necessaryBlocks.Peek();
            }

            return GameStorage.Instance.ActiveCard ? GameStorage.Instance.ActiveCard.Block : null;
        }

        public bool HasNecessaryBlocks() {
            return _necessaryBlocks.Count > 0;
        }

        public void ActiveCardUpdated() {
            if (_necessaryBlocks.Count == 0) {
                GameStorage.Instance.Representer.SetCurrentBlockRepr();
            }
        }
    }
}