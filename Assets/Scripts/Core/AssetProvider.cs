using System;
using System.Collections.Generic;
using Cells;
using Economics;
using ScriptableObjects;
using UnityEngine;

namespace Core
{
    public class AssetProvider : Singleton<AssetProvider>
    {
        public AssetRegistry registry;
        
        private Dictionary<Currency, CurrencySettings> currencies = new();
        private Dictionary<Type, TraitSettings> dynamicTraits = new();
        private Dictionary<CellStaticTraits, TraitSettings> staticTraits = new();

        public CurrencySettings GetCurrency(Currency currency) {
            return currencies[currency];
        }

        public TraitSettings GetTraitInfo(Type trait) {
            Debug.Log(trait.FullName);
            foreach (var pair in dynamicTraits.Keys) {
                Debug.Log(pair);
            };
            return dynamicTraits[trait];
        }

        public TraitSettings GetTraitInfo(CellStaticTraits trait) {
            return !trait.IsSingleFlag() ? throw new ArgumentException($"Trait {trait} is not a single flag") : staticTraits[trait];
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
            
            InitTraits();
        }

        private void InitTraits() {
            // staticTraits[CellStaticTraits.NONE]

            dynamicTraits[typeof(AttackTrait)] = registry.attackTrait;
        }
    }
}