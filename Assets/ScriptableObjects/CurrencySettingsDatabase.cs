using System.Collections.Generic;
using UnityEngine;
using Economics;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Economics/Currency Database")]
	public class CurrencySettingsDatabase : ScriptableObject {
    	public CurrencySettings[] currencies;

    	private Dictionary<Currency, CurrencySettings> _cache;

    	public CurrencySettings Get(Currency c) {
        	if (_cache == null) {
            	_cache = new Dictionary<Currency, CurrencySettings>();
            	foreach (var data in currencies)
               		_cache[data.currency] = data;
        	}
        	return _cache[c];
    	}
	}
}

