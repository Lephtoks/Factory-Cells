using System;

namespace Data
{
    public class MarkedAction
    {
        public event Action Action;
        public bool Dirty { get; private set; }
        
        public static MarkedAction operator +(MarkedAction a, Action b) {
            a.Action += b;
            return a;
        }
        
        public static MarkedAction operator -(MarkedAction a, Action b) {
            a.Action -= b;
            return a;
        }
        
        public void Mark() {
            Dirty = true;
        }

        public void Invoke() {
            if (Dirty) {
                Action?.Invoke();
                Dirty = false;
            }
        }
    }
}