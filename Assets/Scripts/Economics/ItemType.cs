namespace Economics
{
    public class ItemType
    {
        public readonly string Name;

        public ItemType(string name) {
            Name = name;
        }

        public ItemStack OfCount(int count) {
            return new ItemStack(this, count);
        }
    }
}