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
        
        private Dictionary<Type, TraitSettings> dynamicTraits = new();
        private Dictionary<CellStaticTraits, TraitSettings> staticTraits = new();

        public CurrencySettings GetCurrency(Currency currency) {
            foreach (var pair in registry.currencies.currencies) {
                if (pair.type == currency) {
                    return pair.settings;
                }
            }
            throw new ArgumentException($"Currency {currency} has no settings");
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
            
            InitTraits();
        }

        private void InitTraits() {
            // staticTraits[CellStaticTraits.NONE]

            dynamicTraits[typeof(AttackTrait)] = registry.traits.attackTrait;
        }

        public Sprite[] GetConveyorAnimationList(Conveyor conveyor, DirectionFlag connections) {
            switch (conveyor.Direction)
            {
                case Direction.NORTH:
                    return connections.ToByte() switch {
                        4 or 5 => registry.conveyorSprites.ConveyorUpOpenedSprites,
                        8 or 9 => registry.conveyorSprites.ConveyorUpTurnCCWSprites,
                        2 or 3 => registry.conveyorSprites.ConveyorUpTurnCWSprites,
                        10 or 11 => registry.conveyorSprites.ConveyorUpTurnSideCombineSprites,
                        12 or 13 => registry.conveyorSprites.ConveyorUpTurnBackCCWSprites,
                        6 or 7 => registry.conveyorSprites.ConveyorUpTurnBackCWSprites,
                        14 or 15 => registry.conveyorSprites.ConveyorUpTurnCombineSprites,
                        _ => registry.conveyorSprites.ConveyorUpClosedSprites
                    };

                    ;
                case Direction.EAST:
                    return connections.ToByte() switch {
                        1 or 3 => registry.conveyorSprites.ConveyorRightTurnCCWSprites,
                        4 or 6 => registry.conveyorSprites.ConveyorRightTurnCWSprites,
                        9 or 11 => registry.conveyorSprites.ConveyorRightTurnBackCCWSprites,
                        12 or 14 => registry.conveyorSprites.ConveyorRightTurnBackCWSprites,
                        5 or 7 => registry.conveyorSprites.ConveyorRightTurnSideCombineSprites,
                        13 or 15 => registry.conveyorSprites.ConveyorRightTurnCombineSprites,
                        _ => registry.conveyorSprites.ConveyorRightSprites
                    };
                case Direction.WEST:
                    return connections.ToByte() switch {
                        4 or 12 => registry.conveyorSprites.ConveyorLeftTurnCCWSprites,
                        1 or 9 => registry.conveyorSprites.ConveyorLeftTurnCWSprites,
                        6 or 14 => registry.conveyorSprites.ConveyorLeftTurnBackCCWSprites,
                        3 or 11 => registry.conveyorSprites.ConveyorLeftTurnBackCWSprites,
                        5 or 13 => registry.conveyorSprites.ConveyorLeftTurnSideCombineSprites,
                        7 or 15 => registry.conveyorSprites.ConveyorLeftTurnCombineSprites,
                        _ => registry.conveyorSprites.ConveyorRightSprites
                    };
                case Direction.SOUTH:
                    return connections.ToByte() switch {
                        0 or 1 => registry.conveyorSprites.ConveyorDownClosedSprites,
                        2 => registry.conveyorSprites.ConveyorDownTurnCCWClosedSprites,
                        6 => registry.conveyorSprites.ConveyorDownTurnCCWOpenedSprites,
                        8 => registry.conveyorSprites.ConveyorDownTurnCWClosedSprites,
                        12 => registry.conveyorSprites.ConveyorDownTurnCWOpenedSprites,
                        3 => registry.conveyorSprites.ConveyorDownTurnBackCCWClosedSprites,
                        7 => registry.conveyorSprites.ConveyorDownTurnBackCCWOpenedSprites,
                        9 => registry.conveyorSprites.ConveyorDownTurnBackCWClosedSprites,
                        13 => registry.conveyorSprites.ConveyorDownTurnBackCWOpenedSprites,
                        10 => registry.conveyorSprites.ConveyorDownTurnSideCombineClosedSprites,
                        14 => registry.conveyorSprites.ConveyorDownTurnSideCombineOpenedSprites,
                        11 => registry.conveyorSprites.ConveyorDownTurnCombineClosedSprites,
                        15 => registry.conveyorSprites.ConveyorDownTurnCombineOpenedSprites,
                        _ => registry.conveyorSprites.ConveyorUpOpenedSprites
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}