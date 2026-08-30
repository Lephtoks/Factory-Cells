using UnityEngine;

namespace Cells.Object
{
    [CreateAssetMenu(menuName = "Data/Block Definition")]
    public class BlockDefinition : ScriptableObject
    {
        public BlockRepr Representation;
        public Sprite TextureForUI;
        public string Title;
        [TextArea]
        public string Description;
    }
}