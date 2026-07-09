using UnityEngine;

namespace Data
{
    public class CurrencyData
    {
        private int _wind;

        public int Wind
        {
            get => _wind;
            set {
                _wind = value;
                Debug.Log("Wind: " + _wind);
            }
        }
    }
}