namespace Cells.Object
{
    public interface IRepresentable<T, K> where T : BlockRepr<K> where K : Block
    {
        public T Representation {get; }
        public T LivingRepresentation {get; set;}
        
        
        public K AssignRepresentation(T repr) {
            this.LivingRepresentation = repr;
            var cellObject = this as K;
            repr.Init(cellObject);
            return cellObject;
        }
    }
}