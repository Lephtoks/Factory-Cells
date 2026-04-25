namespace Core
{
    public class Singleton<T> : IBootable where T : class
    {
        public static T Instance { get; set; }

        public virtual void Init() {
            Instance = this as T;
        }
    }
}