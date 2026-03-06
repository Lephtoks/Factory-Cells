using System;
using ScriptableObjects;
using UnityEngine;

namespace Global
{
    [DefaultExecutionOrder(-1000)]
    public class GlobalData : MonoBehaviour
    {
        public static GlobalData Instance;

        public CurrencySettingsDatabase currencySettingsDatabase;
        
        public void Awake() {
            Instance = this;
        }
    }
}