using Core.Asset;
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