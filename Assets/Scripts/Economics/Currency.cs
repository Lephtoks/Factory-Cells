namespace Economics
{
    public enum Currency
    {
        AIR,
        STONE,
        WOOD,
        COAL,
        IRON_ORE,
        IRON,
        STONE_HULL,
        PLANKS,
        GOLDEN_ORE,
        GOLD,
        COPPER_ORE,
        COPPER
    }

    public static class CurrencyExtensions
    {
        public static ItemStack OfCount(this Currency currency, int count) {
            return new ItemStack(currency, count);
        }
    }
}