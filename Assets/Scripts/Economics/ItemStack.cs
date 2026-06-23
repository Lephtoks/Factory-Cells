using System;

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

        public ItemStack Add(ItemStack addition, int capacity, out ItemStack added) {
            if (addition.IsEmpty()) {
                added = EMPTY;
                return this;
            }
            if (IsEmpty()) {
                int caped = Math.Min(addition.Count, capacity);
                added = addition.OfCount(caped);
                return added;
            }

            if (CurrencyType == addition.CurrencyType) {
                int newCount = Count + addition.Count;
                int caped = Math.Min(newCount, capacity);
                int dif = caped - Count;
                added = OfCount(dif);
                return OfCount(caped);
            }
            added = EMPTY;
            return this;
        }

        public ItemStack Remove(ItemStack removal, out ItemStack removed) {

            if (removal.IsEmpty() || IsEmpty()) {
                removed = EMPTY;
                return this;
            }
            if (CurrencyType == removal.CurrencyType) {
                removed = removal;                
                return OfCount(Count - removal.Count);
            }
            removed = EMPTY;
            return this;
        }
    }
}