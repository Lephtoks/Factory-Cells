using System;
using System.Collections.Generic;

namespace Core.Asset
{
    [Serializable]
    public class CurrencyAssets
    {
        public List<CurrencyEntry> currencies = new();
    }
}