using System;

namespace Cells
{
    [Flags]
    public enum CellStaticTraits
    {
        NONE = 0
    }

    public static class CellStaticTraitsExtensions
    {
        public static bool IsSingleFlag(this CellStaticTraits value)
        {
            int v = (int)value;
            return (v & (v - 1)) == 0;
        }
    }
}