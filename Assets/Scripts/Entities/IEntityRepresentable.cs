using UnityEngine;

namespace Entities
{
    public interface IEntityRepresentable
    {
        object LivingRepresentationObj { get; }

        void Represent(Entity entity);
    }
    public interface IEntityRepresentable<T, K> : IEntityRepresentable where T : EntityRepr<K> where K : Entity
    {
        public T Representation {get; }
        public T LivingRepresentation {get; set;}

        object IEntityRepresentable.LivingRepresentationObj => LivingRepresentation;


        public K AssignRepresentation(T repr) {
            this.LivingRepresentation = repr;
            var entity = this as K;
            repr.Init(entity);
            return entity;
        }

        void IEntityRepresentable.Represent(Entity entity) {
            LivingRepresentation = Object.Instantiate(Representation);
            LivingRepresentation.Init(entity as K);
        }
    }
}