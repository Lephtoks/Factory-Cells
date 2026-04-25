using UnityEngine;
using Economics;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Economics/Currency Settings")]
	public class CurrencySettings : ScriptableObject {
		public Currency currency;
		public Sprite icon;
		public string displayName;
		public Color color;
	}
}

