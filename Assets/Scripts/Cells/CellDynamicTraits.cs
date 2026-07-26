namespace Cells
{
    public static class CellDynamicTraits
    {
        public static AttackTrait AttackTrait() {
            return new AttackTrait();
        }
    }

    public record AttackTrait
    {
    }
}