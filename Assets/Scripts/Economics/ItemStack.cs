namespace Economics
{
    public struct ItemStack
    {
        public static readonly ItemStack EMPTY = new ItemStack(null, 0);
        public ItemStack(ItemType type, int count) {
            Type = type;
            Count = count;
        }
        public ItemType Type;
        public int Count;

        public bool IsEmpty() {
            return Count == 0 || Type == null;
        }

        public ItemStack OfCount(int count) {
            return new ItemStack(Type, count);
        }
    }
}