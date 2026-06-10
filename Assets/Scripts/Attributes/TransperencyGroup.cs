using UnityEngine;

namespace DefaultNamespace
{
    public class TransparencyGroup : MonoBehaviour
    {
        private SpriteRenderer[] renderers;

        private void Awake()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>(true);
        }

        public void SetAlpha(float alpha)
        {
            foreach (var sr in renderers)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
        }
    }
}