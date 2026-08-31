using System;
using Economics;
using ScriptableObjects;

namespace Core.Asset
{
    [Serializable]
    public class CurrencyEntry
    {
        public Currency type;
        public CurrencySettings settings;
    };
}