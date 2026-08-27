using System.Collections.Generic;
using Cells;
using Cells.Object.Building;
using Cells.Object.Building.Mono;
using Economics;
using Entities.Kinds.Mono;
using ScriptableObjects;
using UI.Cards;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Data/Asset Registry")]
    public class AssetRegistry : ScriptableObject
    {
        public Texture2D playerTexture;
        
        [Header("Conveyor Up")]
        public Sprite[] ConveyorUpClosedSprites;
        public Sprite[] ConveyorUpOpenedSprites; // And ConveyorDownOpened
        public Sprite[] ConveyorUpTurnCCWSprites;
        public Sprite[] ConveyorUpTurnCWSprites;
        public Sprite[] ConveyorUpTurnBackCCWSprites;
        public Sprite[] ConveyorUpTurnBackCWSprites;
        public Sprite[] ConveyorUpTurnSideCombineSprites;
        public Sprite[] ConveyorUpTurnCombineSprites;
        
        [Header("Conveyor Right")]
        public Sprite[] ConveyorRightSprites; // And ConveyorLeft
        public Sprite[] ConveyorRightTurnCCWSprites;
        public Sprite[] ConveyorRightTurnCWSprites;
        public Sprite[] ConveyorRightTurnBackCCWSprites;
        public Sprite[] ConveyorRightTurnBackCWSprites;
        public Sprite[] ConveyorRightTurnSideCombineSprites;
        public Sprite[] ConveyorRightTurnCombineSprites;
        
        [Header("Conveyor Left")]
        public Sprite[] ConveyorLeftTurnCCWSprites;
        public Sprite[] ConveyorLeftTurnCWSprites;
        public Sprite[] ConveyorLeftTurnBackCCWSprites;
        public Sprite[] ConveyorLeftTurnBackCWSprites;
        public Sprite[] ConveyorLeftTurnSideCombineSprites;
        public Sprite[] ConveyorLeftTurnCombineSprites;
        
        [Header("Conveyor Down")]
        public Sprite[] ConveyorDownClosedSprites;
        public Sprite[] ConveyorDownTurnCCWClosedSprites;
        public Sprite[] ConveyorDownTurnCCWOpenedSprites;
        public Sprite[] ConveyorDownTurnCWClosedSprites;
        public Sprite[] ConveyorDownTurnCWOpenedSprites;
        public Sprite[] ConveyorDownTurnBackCCWClosedSprites;
        public Sprite[] ConveyorDownTurnBackCCWOpenedSprites;
        public Sprite[] ConveyorDownTurnBackCWClosedSprites;
        public Sprite[] ConveyorDownTurnBackCWOpenedSprites;
        public Sprite[] ConveyorDownTurnSideCombineClosedSprites;
        public Sprite[] ConveyorDownTurnSideCombineOpenedSprites;
        public Sprite[] ConveyorDownTurnCombineClosedSprites;
        public Sprite[] ConveyorDownTurnCombineOpenedSprites;
        
        [Header("Currency")]
        
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

        public PointRepr pointEntity;

        public Material ItemDropMaterial;
        public Mesh ItemDropMesh;
        
        public TraitSettings attackTrait;
        
        public GameObject traitIconPrefab;
        
        public Material DebugLineMaterial;
    }
}