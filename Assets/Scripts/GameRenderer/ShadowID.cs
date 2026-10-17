using Cells.Object;

namespace GameRenderer
{
    public struct ShadowID
    {
        public Block Block;
        public int Id;
        
        public ShadowID(Block block, int id) {
            Block = block;
            Id = id;
        }
    }
}