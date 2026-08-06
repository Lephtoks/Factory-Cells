using System.Collections.Generic;
using Cells;
using Cells.Object.Building;
using Cells.Object.Building.Mono;
using Economics;
using ScriptableObjects;
using UI.Cards;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Data/Asset Registry")]
    public class AssetRegistry : ScriptableObject
    {
        public Texture2D playerTexture;
        
        public Sprite[] ConveyorRightSprites;
        
        private Dictionary<Currency, CurrencySettings> currencies = new();
        
        public CurrencySettings air;
        public CurrencySettings copper;
        public CurrencySettings copperOre;
        public CurrencySettings stone;
        public CurrencySettings wood;
        public CurrencySettings iron;
        public CurrencySettings ironOre;
        public CurrencySettings planks;
        public CurrencySettings gold;
        public CurrencySettings goldenOre;
        public CurrencySettings coal;
        public CurrencySettings stoneHull;
        
        public Card cardPrefab;
        public Cell cellPrefab;
        public ConveyorRepr conveyor;
        public DrillRepr drill;
        public WindGenRepr windGen;
        public ItemSourceRepr itemSource;

        public Material ItemDropMaterial;
        public Mesh ItemDropMesh;
        
        public TraitSettings attackTrait;
        
        public GameObject traitIconPrefab;
        
        public Material DebugLineMaterial;
    }
}