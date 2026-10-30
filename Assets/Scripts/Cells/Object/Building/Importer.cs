namespace Cells.Object.Building
{
    public class Importer : OneSlotBlock
    {
        public override bool CanReceive => false;
        public override BlockType BlockType => BlockTypes.IMPORTER;
        public Importer() : base() {}
    }
}