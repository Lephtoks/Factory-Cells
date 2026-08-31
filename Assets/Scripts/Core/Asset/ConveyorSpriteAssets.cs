using System;
using UnityEngine;

namespace Core.Asset
{
    [Serializable]
    public class ConveyorSpriteAssets
    {
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
        
    }
}