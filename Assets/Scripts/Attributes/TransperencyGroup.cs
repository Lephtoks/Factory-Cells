using UnityEngine;

namespace DefaultNamespace
{
    public class TransparencyGroup : MonoBehaviour
    {
        private SpriteRenderer[] renderers;

        protected void Awake()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>(true);
        }

        public void SetAlpha(float alpha)
        {
            foreach (var sr in renderers)
            {
                Color c = Color.white;
                c.a = alpha;
                sr.color = c;
            }
        }
        

        public void SetColor(Color color)
        {
            foreach (var sr in renderers)
            {
                sr.color = color;
            }
        }
    }
}