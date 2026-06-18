namespace Cells.Object
{
    public interface IRepresentable
    {
        object LivingRepresentationObj { get; }
    }
    public interface IRepresentable<T, K> : IRepresentable where T : BlockRepr<K> where K : Block
    {
        public T Representation {get; }
        public T LivingRepresentation {get; set;}

        object IRepresentable.LivingRepresentationObj => LivingRepresentation;


        public K AssignRepresentation(T repr) {
            this.LivingRepresentation = repr;
            var cellObject = this as K;
            repr.Init(cellObject);
            return cellObject;
        }
    }
}