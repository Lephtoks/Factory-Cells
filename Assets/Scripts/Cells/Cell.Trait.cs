using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cells
{
    public partial class Cell
    {
        private CellStaticTraits _staticTraits;
        private readonly Dictionary<Type, object> _dynamicTraits = new();
        
        public bool HasTrait(CellStaticTraits trait) {
            return (_staticTraits & trait) == trait;
        }

        public bool HasTrait<T>() {
            return _dynamicTraits.ContainsKey(typeof(T));
        }
        
        public void AddTrait(CellStaticTraits trait) {
            _staticTraits |= trait;
        }
        
        public void AddTrait([NotNull] object trait) {
            _dynamicTraits[trait.GetType()] = trait;
        }

        public void RemoveTrait(CellStaticTraits trait) {
            _staticTraits &= ~trait;
        }

        public void RemoveTrait([NotNull] object trait) {
            _dynamicTraits.Remove(trait.GetType());
        }
    }
}