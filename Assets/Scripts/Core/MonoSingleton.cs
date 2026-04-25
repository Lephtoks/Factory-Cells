using UnityEngine;

namespace Core
{
    public class MonoSingleton<T> : MonoBehaviour where T : class
    {
        public static T Instance;
        public virtual void Awake() {
            Instance = this as T;
        }
    }
}