using System.Collections.Generic;
using Economics;
using ScriptableObjects;
using UnityEngine;

namespace Core
{
    public class AssetProvider : Singleton<AssetProvider>
    {
        public AssetRegistry registry;
        
        private Dictionary<Currency, CurrencySettings> currencies = new();

        public CurrencySettings GetCurrency(Currency currency) {
            return currencies[currency];
        }

        public override void Init() {
            base.Init();
            registry = Resources.Load<AssetRegistry>("ScriptableObjects/Assets/AssetRegistry");
            currencies[Currency.AIR] = registry.air;
            currencies[Currency.COAL] = registry.coal;
            currencies[Currency.COPPER] = registry.copper;
            currencies[Currency.COPPER_ORE] = registry.copperOre;
            currencies[Currency.GOLDEN_ORE] = registry.goldenOre;
            currencies[Currency.GOLD] = registry.gold;
            currencies[Currency.IRON] = registry.iron;
            currencies[Currency.IRON_ORE] = registry.ironOre;
            currencies[Currency.PLANKS] = registry.planks;
            currencies[Currency.STONE] = registry.stone;
            currencies[Currency.STONE_HULL] = registry.stoneHull;
            currencies[Currency.WOOD] = registry.wood;
        }
    }
}