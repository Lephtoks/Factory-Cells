using System;
using System.Collections.Generic;
using Cells;
using Cells.Object.Building;
using Data;
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

        public Sprite[] GetConveyorAnimationList(Conveyor conveyor, DirectionFlag connections) {
            switch (conveyor.Direction)
            {
                case Direction.NORTH:
                    return connections.ToByte() switch {
                        4 or 5 => registry.ConveyorUpOpenedSprites,
                        8 or 9 => registry.ConveyorUpTurnCCWSprites,
                        2 or 3 => registry.ConveyorUpTurnCWSprites,
                        10 or 11 => registry.ConveyorUpTurnSideCombineSprites,
                        12 or 13 => registry.ConveyorUpTurnBackCCWSprites,
                        6 or 7 => registry.ConveyorUpTurnBackCWSprites,
                        14 or 15 => registry.ConveyorUpTurnCombineSprites,
                        _ => registry.ConveyorUpClosedSprites
                    };

                    ;
                case Direction.EAST:
                    return connections.ToByte() switch {
                        1 or 3 => registry.ConveyorRightTurnCCWSprites,
                        4 or 6 => registry.ConveyorRightTurnCWSprites,
                        9 or 11 => registry.ConveyorRightTurnBackCCWSprites,
                        12 or 14 => registry.ConveyorRightTurnBackCWSprites,
                        5 or 7 => registry.ConveyorRightTurnSideCombineSprites,
                        13 or 15 => registry.ConveyorRightTurnCombineSprites,
                        _ => registry.ConveyorRightSprites
                    };
                case Direction.WEST:
                    return connections.ToByte() switch {
                        4 or 12 => registry.ConveyorLeftTurnCCWSprites,
                        1 or 9 => registry.ConveyorLeftTurnCWSprites,
                        6 or 14 => registry.ConveyorLeftTurnBackCCWSprites,
                        3 or 11 => registry.ConveyorLeftTurnBackCWSprites,
                        5 or 13 => registry.ConveyorLeftTurnSideCombineSprites,
                        7 or 15 => registry.ConveyorLeftTurnCombineSprites,
                        _ => registry.ConveyorRightSprites
                    };
                case Direction.SOUTH:
                    return connections.ToByte() switch {
                        0 or 1 => registry.ConveyorDownClosedSprites,
                        2 => registry.ConveyorDownTurnCCWClosedSprites,
                        6 => registry.ConveyorDownTurnCCWOpenedSprites,
                        8 => registry.ConveyorDownTurnCWClosedSprites,
                        12 => registry.ConveyorDownTurnCWOpenedSprites,
                        3 => registry.ConveyorDownTurnBackCCWClosedSprites,
                        7 => registry.ConveyorDownTurnBackCCWOpenedSprites,
                        9 => registry.ConveyorDownTurnBackCWClosedSprites,
                        13 => registry.ConveyorDownTurnBackCWOpenedSprites,
                        10 => registry.ConveyorDownTurnSideCombineClosedSprites,
                        14 => registry.ConveyorDownTurnSideCombineOpenedSprites,
                        11 => registry.ConveyorDownTurnCombineClosedSprites,
                        15 => registry.ConveyorDownTurnCombineOpenedSprites,
                        _ => registry.ConveyorUpOpenedSprites
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}