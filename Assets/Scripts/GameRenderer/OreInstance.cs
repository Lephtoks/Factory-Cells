using System;
using Economics;
using UnityEngine;
using Random = System.Random;

namespace GameRenderer
{
    public struct OreInstance
    {
        public static Random Random = new Random();
        Vector2Int Position;
        int Index;

        public Currency OreType
        {
            get
            {
                return Index switch {
                    <= 2 => Currency.STONE,
                    <= 5 => Currency.COPPER_ORE,
                    _ => throw new ArgumentException("Unknown Ore Type")
                };
            }
        }

        public OreInstance(Vector2Int position, Currency currency) {
            Index = currency switch {
                Currency.STONE => Random.Next(0, 2),
                Currency.COPPER_ORE => Random.Next(3, 5),
                _ => throw new ArgumentException($"Currency {currency} does not have a place in ore atlas")
            };
            Position = position;
        }
    }
}