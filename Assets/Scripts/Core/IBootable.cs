namespace Core
{
    public interface IBootable
    {
        public void Init() {}

        public virtual void Dispose() { }
    }
}