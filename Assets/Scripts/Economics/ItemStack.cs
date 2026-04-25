namespace Economics
{
    public struct ItemStack
    {
        public static readonly ItemStack EMPTY = new(Currency.AIR, 0);
        public ItemStack(Currency type, int count) {
            CurrencyType = type;
            Count = count;
        }
        public Currency CurrencyType;
        public int Count;

        public bool IsEmpty() {
            return Count == 0 || CurrencyType == Currency.AIR;
        }

        public ItemStack OfCount(int count) {
            return count <= 0 ? EMPTY : new ItemStack(CurrencyType, count);
        }
    }
}