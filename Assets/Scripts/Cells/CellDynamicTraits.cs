namespace Cells
{
    public class CellDynamicTraits
    {
        public AttackTrait AttackTrait() {
            return new AttackTrait();
        }
    }

    public record AttackTrait
    {
    }
}