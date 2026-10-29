using UnityEngine;

namespace GameRenderer
{
    [CreateAssetMenu(menuName = "Data/Block Animation")]
    public class BlockAnimation : ScriptableObject
    {
        public Sprite[] Sprites;
        public Vector2 Scale;
        public float Speed = 1f;
    }
}
