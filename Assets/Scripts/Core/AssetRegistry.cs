using System.Collections.Generic;
using Cells;
using Cells.Object;
using Cells.Object.Building;
using Cells.Object.Building.Mono;
using Core.Asset;
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
        public BlockAssets blocks;
        public ConveyorSpriteAssets conveyorSprites;
        public CardAssets cards;
        public CellAssets cells;
        public CurrencyAssets currencies;
        public EntityAssets entities;
        public TraitAssets traits;
        public RenderAssets render;
    }
}