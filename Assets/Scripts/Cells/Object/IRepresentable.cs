namespace Cells.Object
{
    public interface IRepresentable
    {
        object LivingRepresentationObj { get; }
    }
    public interface IRepresentable<T> : IRepresentable where T : BlockRepr
    {
        public T LivingRepresentation {get; set;}

        object IRepresentable.LivingRepresentationObj => LivingRepresentation;


        public Block AssignRepresentation(BlockRepr repr) {
            var block = (Block)this;
            LivingRepresentation = (T) repr;
            LivingRepresentation.Init(block);
            return block;
        }
    }
}